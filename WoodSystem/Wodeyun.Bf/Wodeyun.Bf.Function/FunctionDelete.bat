path "C:\Windows\System32\inetsrv"

appcmd delete apppool "Wodeyun.Bf.Function.Services"
appcmd delete site "Wodeyun.Bf.Function.Services"

appcmd delete apppool "Wodeyun.Bf.Function.Web"
appcmd delete site "Wodeyun.Bf.Function.Web"
