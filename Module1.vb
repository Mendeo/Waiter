Option Explicit On
Option Strict On

Module Module1
	Private SETTINGS_FILE_NAME As String = "settings.txt"
	Sub Main()
		Dim sr As New IO.StreamReader(IO.Path.Combine(My.Application.Info.DirectoryPath, SETTINGS_FILE_NAME))

		Dim procsOrFiles() As String = sr.ReadLine().Split(";"c)
		Dim isFiles As Boolean = False
		For Each entry As String In procsOrFiles
			If entry.Contains(":") Then
				isFiles = True
				Exit For
			End If
		Next
		Dim period As Integer = Integer.Parse(sr.ReadLine())
		Dim timeAfterTrigger As Integer = Integer.Parse(sr.ReadLine())
		Dim testFunction As TestFunction
		If isFiles Then
			testFunction = AddressOf hasNoFile
		Else
			testFunction = AddressOf hasProc
		End If
		Do
			If test(procsOrFiles, testFunction) Then
				Console.Write("+")
				Threading.Thread.Sleep(timeAfterTrigger) 'Ждём, вдруг появится
				If test(procsOrFiles, testFunction) Then Exit Do
			End If
			Console.Write("-")
			Threading.Thread.Sleep(period)
		Loop
		Threading.Thread.Sleep(1000)
		Dim sw As New IO.StreamWriter("time.txt")
		sw.WriteLine(Date.Now.ToLongDateString & " " & Date.Now.ToLongTimeString)
		sw.Flush()
		sw.Close()
		Shell("shutdown /s /f /t 0")
	End Sub

	Private Function hasNoFile(file As String) As Boolean
		Return Not IO.File.Exists(file)
	End Function
	Private Function hasProc(procName As String) As Boolean
		Return Process.GetProcessesByName(procName).Length > 0
	End Function
	Delegate Function TestFunction(name As String) As Boolean
	Private Function test(procsOrFiles() As String, tf As TestFunction) As Boolean
		Dim flag As Boolean = True
		For Each entry As String In procsOrFiles
			flag = flag And Not tf(entry)
			If Not flag Then Exit For
		Next
		Return flag
	End Function
End Module
