path "C:\Windows\System32\inetsrv"

appcmd delete apppool "Wodeyun.Project.GsmLine.Services"
appcmd delete site "Wodeyun.Project.GsmLine.Services"

appcmd delete apppool "Wodeyun.Project.GsmLine.Web"
appcmd delete site "Wodeyun.Project.GsmLine.Web"
