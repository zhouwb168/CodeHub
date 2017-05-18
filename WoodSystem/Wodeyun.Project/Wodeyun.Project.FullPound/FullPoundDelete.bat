path "C:\Windows\System32\inetsrv"

appcmd delete apppool "Wodeyun.Project.FullPound.Services"
appcmd delete site "Wodeyun.Project.FullPound.Services"

appcmd delete apppool "Wodeyun.Project.FullPound.Web"
appcmd delete site "Wodeyun.Project.FullPound.Web"
