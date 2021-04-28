Option Explicit On
Option Strict On

Module Module1
	Private SETTINGS_FILE_NAME As String = "settings.txt"
	Sub Main()
		Dim sr As New IO.StreamReader(IO.Path.Combine(My.Application.Info.DirectoryPath, SETTINGS_FILE_NAME))
		Dim procNames() As String = sr.ReadLine().Split(" "c)
		Dim period As Integer = Integer.Parse(sr.ReadLine())
		Do
			If noOneProcs(procNames) Then
				Console.Write("+")
				Threading.Thread.Sleep(3000) 'Ждём, вдруг появится
				If noOneProcs(procNames) Then Exit Do
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

	Private Function hasProc(procName As String) As Boolean
		Return Process.GetProcessesByName(procName).Length > 0
	End Function
	Private Function noOneProcs(procNames() As String) As Boolean
		Dim flag As Boolean = True
		For Each proc As String In procNames
			flag = flag And Not hasProc(proc)
			If Not flag Then Exit For
		Next
		Return flag
	End Function
End Module
