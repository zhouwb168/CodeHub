path "C:\Windows\System32\inetsrv"

appcmd delete apppool "Wodeyun.Bf.Verificate.Services"
appcmd delete site "Wodeyun.Bf.Verificate.Services"

appcmd delete apppool "Wodeyun.Bf.Verificate.Web"
appcmd delete site "Wodeyun.Bf.Verificate.Web"
