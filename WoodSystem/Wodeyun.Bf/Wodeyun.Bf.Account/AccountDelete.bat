path "C:\Windows\System32\inetsrv"

appcmd delete apppool "Wodeyun.Bf.Account.Services"
appcmd delete site "Wodeyun.Bf.Account.Services"

appcmd delete apppool "Wodeyun.Bf.Account.Web"
appcmd delete site "Wodeyun.Bf.Account.Web"
