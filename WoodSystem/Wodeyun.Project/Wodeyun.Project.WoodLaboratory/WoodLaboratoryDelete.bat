path "C:\Windows\System32\inetsrv"

appcmd delete apppool "Wodeyun.Project.WoodLaboratory.Services"
appcmd delete site "Wodeyun.Project.WoodLaboratory.Services"

appcmd delete apppool "Wodeyun.Project.WoodLaboratory.Web"
appcmd delete site "Wodeyun.Project.WoodLaboratory.Web"
