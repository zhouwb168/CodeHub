path "C:\Windows\System32\inetsrv"

appcmd delete apppool "Wodeyun.Project.WoodFinance.Services"
appcmd delete site "Wodeyun.Project.WoodFinance.Services"

appcmd delete apppool "Wodeyun.Project.WoodFinance.Web"
appcmd delete site "Wodeyun.Project.WoodFinance.Web"
