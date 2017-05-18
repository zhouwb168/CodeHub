path "C:\Windows\System32\inetsrv"

appcmd delete apppool "Wodeyun.Project.Check.Services"
appcmd delete site "Wodeyun.Project.Check.Services"

appcmd delete apppool "Wodeyun.Project.Check.Web"
appcmd delete site "Wodeyun.Project.Check.Web"
