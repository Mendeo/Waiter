Option Explicit On
Option Strict On

Module Module1
	Private SETTINGS_FILE_NAME As String = "settings.txt"
	Sub Main()
		Dim sr As New IO.StreamReader(IO.Path.Combine(My.Application.Info.DirectoryPath, SETTINGS_FILE_NAME))
		Dim procNames() As String = sr.ReadLine().Split(" "c)
		Dim period As Integer = Integer.Parse(sr.ReadLine())
		Do
			Dim flag As Boolean = True
			For Each proc As String In procNames
				flag = flag And (Process.GetProcessesByName(proc).Length = 0)
			Next
			If flag Then Exit Do
			Threading.Thread.Sleep(period)
		Loop
		Threading.Thread.Sleep(1000)
		Dim sw As New IO.StreamWriter("time.txt")
		sw.WriteLine(Date.Now.ToLongDateString & " " & Date.Now.ToLongTimeString)
		sw.Flush()
		sw.Close()
		Shell("shutdown /s /f /t 0")
	End Sub

End Module
