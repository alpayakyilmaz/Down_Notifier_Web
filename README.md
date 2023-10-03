# Down_Notifier_Web
uygulama da requestleri ve exceptionları loglama için Elmah kütüphanesi kullanılmıştır. http:// localhost/4567/Elmah şeklinde elmah ara yüzüne ulaşabilirsiniz.
istek yapılmak istenen Url ler form ara yüzünden eklenir ardından belirlenen periyıtlarda ilgili url' e background jop ile sürekli kontrol edilir.
Url ayakta değil ya da hatalı ise log exception atar ve ilglili tabloya yazılır ve mail atılır. burada mail yöntemi kullandım ama sms ya da Notification da olabilir.
Mail configusayonu appsetting.json dosyasında kendinize göre değiştirip mail alabilirsiniz


Uygulama Entity Framework Code first yazıldığı için Nuget Packege Manager Consol üzeriden update-database komutunu çalıştırarak projenin veri tabanını oluşturun

Uygulama girilen Uri bilgisini sürekli dinler ve uygulamanın bilgilerini belirlenen aralıkta HealthCheckLogs tablosuna yazar ancak ayakta olmayan ya da yanlış girilmiş Uri bilgisi varsa ya da status code 200 değilse 
bunu yine aynı tabloya yazmak ile birikte HealthCheckMailLogs tablsuna hatalı Url bilgisini yazar ve mail atar 

