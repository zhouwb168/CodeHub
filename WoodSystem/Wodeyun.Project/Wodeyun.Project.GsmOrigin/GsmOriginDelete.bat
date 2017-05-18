path "C:\Windows\System32\inetsrv"

appcmd delete apppool "Wodeyun.Project.GsmOrigin.Services"
appcmd delete site "Wodeyun.Project.GsmOrigin.Services"

appcmd delete apppool "Wodeyun.Project.GsmOrigin.Web"
appcmd delete site "Wodeyun.Project.GsmOrigin.Web"
