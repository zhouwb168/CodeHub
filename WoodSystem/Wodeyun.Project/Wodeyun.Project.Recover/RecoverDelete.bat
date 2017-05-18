path "C:\Windows\System32\inetsrv"

appcmd delete apppool "Wodeyun.Project.Recover.Services"
appcmd delete site "Wodeyun.Project.Recover.Services"

appcmd delete apppool "Wodeyun.Project.Recover.Web"
appcmd delete site "Wodeyun.Project.Recover.Web"
