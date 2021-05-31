# Worker Service Url Polling 📎
Simple Windows Service that polls URLs

To install as Windows Service
1. Publish

2. Run command in PowerShell
   ```sh 
   New-Service -Name {SERVICE NAME} -BinaryPathName {EXE FILE PATH} -Description "{DESCRIPTION}" -DisplayName "{DISPLAY NAME}" -StartupType Automatic 
   ```
   
<br>

To change polling URL, edit line 16 in ```./WorkerService/Worker.cs```
```c# class:"lineNo"
private List<String> Urls = new List<string> { "https://www.google.com/" };
```
