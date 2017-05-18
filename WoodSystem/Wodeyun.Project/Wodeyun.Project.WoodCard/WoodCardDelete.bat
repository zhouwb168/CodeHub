path "C:\Windows\System32\inetsrv"

appcmd delete apppool "Wodeyun.Project.WoodCard.Services"
appcmd delete site "Wodeyun.Project.WoodCard.Services"

appcmd delete apppool "Wodeyun.Project.WoodCard.Web"
appcmd delete site "Wodeyun.Project.WoodCard.Web"
