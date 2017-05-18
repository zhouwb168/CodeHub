path "C:\Windows\System32\inetsrv"

appcmd delete apppool "Wodeyun.Project.WoodLaboratoryConfirme.Services"
appcmd delete site "Wodeyun.Project.WoodLaboratoryConfirme.Services"

appcmd delete apppool "Wodeyun.Project.WoodLaboratoryConfirme.Web"
appcmd delete site "Wodeyun.Project.WoodLaboratoryConfirme.Web"
