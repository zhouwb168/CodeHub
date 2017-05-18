path "C:\Windows\System32\inetsrv"

appcmd delete apppool "Wodeyun.Project.EmptyPound.Services"
appcmd delete site "Wodeyun.Project.EmptyPound.Services"

appcmd delete apppool "Wodeyun.Project.EmptyPound.Web"
appcmd delete site "Wodeyun.Project.EmptyPound.Web"
