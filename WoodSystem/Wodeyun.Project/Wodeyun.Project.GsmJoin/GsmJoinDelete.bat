path "C:\Windows\System32\inetsrv"

appcmd delete apppool "Wodeyun.Project.GsmJoin.Services"
appcmd delete site "Wodeyun.Project.GsmJoin.Services"

appcmd delete apppool "Wodeyun.Project.GsmJoin.Web"
appcmd delete site "Wodeyun.Project.GsmJoin.Web"
