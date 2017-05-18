path "C:\Windows\System32\inetsrv"

appcmd delete apppool "Wodeyun.Bf.Grant.Services"
appcmd delete site "Wodeyun.Bf.Grant.Services"

appcmd delete apppool "Wodeyun.Bf.Grant.Web"
appcmd delete site "Wodeyun.Bf.Grant.Web"
