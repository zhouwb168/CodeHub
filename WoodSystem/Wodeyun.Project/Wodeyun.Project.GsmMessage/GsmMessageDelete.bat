path "C:\Windows\System32\inetsrv"

appcmd delete apppool "Wodeyun.Project.GsmMessage.Services"
appcmd delete site "Wodeyun.Project.GsmMessage.Services"

appcmd delete apppool "Wodeyun.Project.GsmMessage.Web"
appcmd delete site "Wodeyun.Project.GsmMessage.Web"
