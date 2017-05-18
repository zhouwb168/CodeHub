path "C:\Windows\System32\inetsrv"

appcmd delete apppool "Wodeyun.Project.WoodPowerOfReadCard.Services"
appcmd delete site "Wodeyun.Project.WoodPowerOfReadCard.Services"

appcmd delete apppool "Wodeyun.Project.WoodPowerOfReadCard.Web"
appcmd delete site "Wodeyun.Project.WoodPowerOfReadCard.Web"
