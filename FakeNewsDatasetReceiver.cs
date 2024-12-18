using RestSharp;
using System.Xml;

string csvFile = "id,title,text,label\n";
int id = 0;

string[] uris = new string[] {
    "https://www.milliyet.com.tr/rss/rssnew/dunyarss.xml",
    "https://www.milliyet.com.tr/rss/rssnew/ekonomi.xml",
    "https://www.milliyet.com.tr/rss/rssnew/siyasetrss.xml",
    "https://www.milliyet.com.tr/rss/rssnew/magazinrss.xml",
    "https://www.milliyet.com.tr/rss/rssnew/gundem.xml",
    "https://www.milliyet.com.tr/rss/rssnew/otomobilrss.xml",
    "https://www.milliyet.com.tr/rss/rssnew/teknolojirss.xml",
    "https://www.milliyet.com.tr/rss/rssnew/egitim.xml",
    "https://www.milliyet.com.tr/rss/rssnew/milliyettatilrss.xml",
    "https://www.milliyet.com.tr/rss/rssnew/modarss.xml",
    "https://www.milliyet.com.tr/rss/rssnew/guzellikrss.xml",
    "https://www.milliyet.com.tr/rss/rssnew/ailerss.xml",
    "https://www.milliyet.com.tr/rss/rssnew/saglik.xml",
    "https://www.milliyet.com.tr/rss/rssnew/yemekrss.xml",
    "https://www.milliyet.com.tr/rss/rssnew/diyet.xml",
    "https://www.milliyet.com.tr/rss/rssnew/iliskiler.xml",
    "https://www.milliyet.com.tr/rss/rssnew/dekorasyonrss.xml",
    "https://www.milliyet.com.tr/rss/rssnew/yasamrss.xml",
    "https://www.milliyet.com.tr/rss/rssnew/astrolojirss.xml",
    "https://www.milliyet.com.tr/rss/rssnew/sondakikarss.xml",
    "https://www.milliyet.com.tr/rss/rssnew/yazarlarrss.xml",
};

foreach (var uri in uris)
{
    RestClient restClient = new RestClient();
    RestRequest restRequest = new RestRequest(uri);
    var response = restClient.Execute(restRequest);

    XmlDocument xmlDocument = new XmlDocument();
    xmlDocument.LoadXml(response.Content);

    foreach (XmlNode news in xmlDocument.GetElementsByTagName("item"))
    {
        XmlDocument descDoc;
        try
        {
            descDoc = new XmlDocument();
            descDoc.LoadXml($"<root>{news["description"].InnerText.Replace("&", "&amp;")}</root>");
        }
        catch (Exception)
        {
            continue;
        }

        string description = string.Empty;
        XmlNodeList paragraphs = descDoc.SelectNodes("//p");
        foreach (XmlNode p in paragraphs)
            if (!string.IsNullOrWhiteSpace(p.InnerText))
                description += p.InnerText;

        csvFile += $"{id++},{news["title"].InnerText.Replace(',', ';')},{description.Replace(',', ';')},0\n";
    }
}

var fakeNews = new List<dynamic>()
{
    new { Title = "Uzaylılar Mars'ta Koloni Kurdu", Description = "Bilim insanları Mars'ta uzaylılara ait olduğu düşünülen bir koloni keşfetti." },
    new { Title = "Dünyanın Çekirdeği Soğumaya Başladı", Description = "Jeologlar dünyanın çekirdeğinin beklenmedik şekilde soğumaya başladığını açıkladı." },
    new { Title = "Zaman Yolcusu Gelecekten Haber Getirdi", Description = "Bir zaman yolcusu 2050 yılından geldiğini iddia ederek geleceğe dair açıklamalar yaptı." },
    new { Title = "Yapay Zeka İnsanlık İçin Başkan Adayı Oldu", Description = "Bir yapay zeka sisteminin dünya başkanlığına aday olduğu duyuruldu." },
    new { Title = "Atlantis'in Kalıntıları Bulundu", Description = "Okyanus araştırmacıları, kayıp kıta Atlantis'e ait olduğu düşünülen kalıntılar keşfetti." },
    new { Title = "Yürüyen Ağaçlar Ormanda Görüldü", Description = "Bir grup doğa gezgini, kendi başına hareket eden ağaçlara tanık olduklarını iddia etti." },
    new { Title = "Ay'da Yeni Bir Madde Keşfedildi", Description = "Astronotlar Ay yüzeyinde daha önce bilinmeyen yeni bir madde tespit etti." },
    new { Title = "Bir Günlük İnternetsiz Dünya Denemesi Yapıldı", Description = "Dünya genelinde bir günlüğüne internet erişimi kapatılarak sonuçlar analiz edildi." },
    new { Title = "Kedilerin İnsanları Yönetme Planı Ortaya Çıktı", Description = "Bilim insanları kedilerin davranışlarının insanları yönetmeye yönelik olduğunu açıkladı." },
    new { Title = "Gizli Piramitler Çölün Altında Keşfedildi", Description = "Arkeologlar çölün derinliklerinde gömülü olan gizli piramitleri ortaya çıkardı." },
    new { Title = "İnsanların Okuma Hızı Artış Gösterdi", Description = "Yeni bir beyin egzersizi sayesinde insanların okuma hızında büyük bir artış görüldü." },
    new { Title = "Dünya Yörüngesinde Bilinmeyen Cisim Görüldü", Description = "Astronomlar, dünyanın yörüngesinde hareket eden bilinmeyen bir cisim tespit etti." },
    new { Title = "Evrenin Sınırında Gizemli Enerji Kaynağı", Description = "Uzay teleskopları, evrenin sınırında yeni bir enerji kaynağı tespit etti." },
    new { Title = "Yeni Dilde 1000 Kelime Öğrenmek 1 Saat Sürecek", Description = "Dil bilimciler yeni geliştirdikleri bir dille 1000 kelimenin 1 saatte öğrenilebileceğini açıkladı." },
    new { Title = "Balinalar İnsanlarla İletişime Geçmeye Çalışıyor", Description = "Okyanus araştırmacıları, balinaların insanlarla iletişime geçme çabalarını belgeledi." },
    new { Title = "Robotlar Sanat Sergisi Açtı", Description = "Bir grup yapay zeka ve robot, tamamen kendileri tarafından hazırlanan bir sanat sergisi açtı." },
    new { Title = "Küçük Bir Ada Gece Aniden Kayboldu", Description = "Uydu görüntüleri, küçük bir adanın gece aniden kaybolduğunu ortaya koydu." },
    new { Title = "Bilinmeyen Bir Hastalık Yayılmaya Başladı", Description = "Sağlık otoriteleri, dünyada bilinmeyen bir hastalığın yayılmaya başladığını açıkladı." },
    new { Title = "Denizlerin Dibinde Devasa Canlı Keşfedildi", Description = "Denizaltı araştırmaları sırasında okyanusun dibinde devasa bir canlı keşfedildi." },
    new { Title = "Yapay Zeka Kendi Dilini Oluşturdu", Description = "Araştırmacılar, yapay zekanın insanların anlayamayacağı bir dil oluşturduğunu keşfetti." },
    new { Title = "Yeni Tür Bir Bitki Et Yemeğe Başladı", Description = "Botanikçiler, et yiyen yeni bir bitki türünü tropikal ormanlarda keşfetti." },
    new { Title = "Bilim İnsanları Rüyaların Kaydedilebileceğini Açıkladı", Description = "Yeni bir teknolojiyle rüyaların görüntü olarak kaydedilebileceği duyuruldu." },
    new { Title = "Kuşlar İnsanların Konuşmasını Taklit Ediyor", Description = "Bilim insanları, bazı kuş türlerinin insan konuşmalarını doğru şekilde taklit edebildiğini belirledi." },
    new { Title = "Buzullarda Antik Canlılar Canlanıyor", Description = "Isınan buzullarda milyonlarca yıl öncesine ait canlıların yeniden canlandığı tespit edildi." },
    new { Title = "Denizler Altında Kaybolmuş Bir Şehir Bulundu", Description = "Dalgıçlar, okyanusun derinliklerinde kayıp bir şehre ait kalıntılar keşfetti." },
    new { Title = "Yürüyen Balık Görüntülendi", Description = "Araştırmacılar, yüzmek yerine yürüme yeteneğine sahip bir balık türünü keşfetti." },
    new { Title = "Ay'da Buzdan Mağaralar Keşfedildi", Description = "Astronotlar, Ay'ın yüzeyinde devasa buz mağaraları bulduğunu açıkladı." },
    new { Title = "Tarihi Eserlerden Gelen Gizemli Sesler", Description = "Arkeologlar, eski tapınaklarda duyulan gizemli seslerin kaynağını araştırıyor." },
    new { Title = "Bulutların Üzerinde Görülen Gölge Şehir", Description = "Bazı bölgelerde bulutların üzerinde gölge şeklinde görülen şehirler dikkat çekiyor." },
    new { Title = "Yüzen Adalar Okyanuslarda Gezmeye Başladı", Description = "Uydu görüntüleri, okyanuslarda hareket eden yüzen adalar olduğunu gösterdi." },
    new { Title = "Bitkilerle Konuşma Teknolojisi Geliştirildi", Description = "Mühendisler, bitkilerle iletişim kurmaya yarayan bir cihaz geliştirdi." },
    new { Title = "Zamanı Durdurabilen Saat Üretildi", Description = "Yeni geliştirilen bir saatin zamanı kısmen durdurabildiği iddia edildi." },
    new { Title = "Kendi Kendini Onaran Evler İnşa Ediliyor", Description = "Mimarlar, hasar gördüğünde kendini onaran malzemelerden evler inşa etmeye başladı." },
    new { Title = "Gökyüzünde Devasa Bir Göz Görüldü", Description = "Bir grup astronom, gökyüzünde devasa bir göz şekli tespit etti." },
    new { Title = "Suyun Altında Nefes Alabilen İnsanlar", Description = "Bazı insanların su altında oksijensiz nefes alabildiği keşfedildi." },
    new { Title = "Dünya'nın Yüzeyinde Kendiliğinden Açılan Kapılar", Description = "Bilim insanları, bazı bölgelerde kendiliğinden açılan enerji kapıları gözlemledi." },
    new { Title = "Bir Gecede Büyüyen Devasa Dağ", Description = "Bir bölgede, bir gecede ortaya çıkan devasa bir dağ şok etkisi yarattı." },
    new { Title = "Zamanın Geriye Aktığı Bölge Keşfedildi", Description = "Araştırmacılar, zamanın geriye aktığı gizemli bir bölgeyi keşfettiklerini duyurdu." },
    new { Title = "Robotların Duygusal Zeka Testi Başarıyla Geçti", Description = "Yeni nesil robotlar, insanların duygularını anlayarak doğru tepkiler vermeyi başardı." },
    new { Title = "İnsan DNA'sı ile Bitki DNA'sı Birleştirildi", Description = "Genetik mühendisler, insan ve bitki DNA'sını birleştirerek oksijen üreten bir canlı oluşturdu." },
    new { Title = "Kayıp Kıta Pasifik Okyanusunda Bulundu", Description = "Denizaltı araştırmaları sırasında milyonlarca yıl önce kaybolduğu düşünülen bir kıta keşfedildi." },
    new { Title = "Uçan Otobüsler Trafik Sorununu Çözecek", Description = "Şehirlerde trafik sorununun çözümü için uçan otobüslerin test sürüşleri başladı." },
    new { Title = "İnsan Beyni ile Kontrol Edilen Arabalar Tanıtıldı", Description = "Yeni teknoloji sayesinde, arabalar sadece düşünce gücüyle kontrol edilebiliyor." },
    new { Title = "Gökten Yumurta Şeklinde Taşlar Yağdı", Description = "Bir kasabada gökten düşen yumurta şeklindeki taşlar bilim dünyasını şaşırttı." },
    new { Title = "Deniz Altında Parlayan Piramitler Keşfedildi", Description = "Dalgıçlar, okyanusun derinliklerinde ışık saçan antik piramitler buldu." },
    new { Title = "Hızla Gelişen Şehirler Gökyüzüne Taşınıyor", Description = "Mimarlar, gökyüzünde devasa platformlar üzerine şehirler inşa etmeyi planlıyor." },
    new { Title = "Hayvanlarla Telepatik İletişim Kuruldu", Description = "Bilim insanları, hayvanlarla düşünce yoluyla iletişim kurabilen bir cihaz geliştirdi." },
    new { Title = "Mars'ta Solunabilir Hava Üretildi", Description = "Mars'ta oksijen üretmeyi başaran bilim insanları, kolonileşmenin önünü açtı." },
    new { Title = "Zehirli Mantarlar Altın Üretmeye Başladı", Description = "Araştırmacılar, bazı zehirli mantar türlerinin altın partikülleri üretebildiğini keşfetti." },
    new { Title = "Dünya'nın Yerçekimi Aniden Değişti", Description = "Bazı bölgelerde yerçekimi anormal değişiklikler göstererek gündelik hayatı etkiledi." },
    new { Title = "Yapay Güneş Enerji Sorununu Bitirecek", Description = "Bilim insanları, yapay bir güneş oluşturarak sınırsız enerji kaynağı elde etmeyi hedefliyor." },
    new { Title = "İnsan Gözüyle Görünemeyen Renkler Tespit Edildi", Description = "Araştırmacılar, insan gözünün algılayamadığı yeni renklerin varlığını doğruladı." },
    new { Title = "Düşüncelerle Yazı Yazılabilen Kalem Tanıtıldı", Description = "Yeni geliştirilen kalem, insanların düşüncelerini anında yazıya dökebiliyor." },
    new { Title = "Okyanus Dibinde Hareket Eden Dağlar Keşfedildi", Description = "Denizaltı gözlemleri, okyanusun dibinde hareket eden devasa dağların varlığını ortaya çıkardı." },
    new { Title = "Zamansız Yaşlanma Hastalığına Çare Bulundu", Description = "Bilim insanları, yaşlanmayı tamamen durdurabilen bir tedavi yöntemi geliştirdi." },
    new { Title = "Gökyüzünde Dev Balık Sürüleri Görüldü", Description = "Bazı bölgelerde gökyüzünde uçan dev balık sürüleri gözlemlendi." },
    new { Title = "Kendi Kendini Çoğaltan Robotlar Yapıldı", Description = "Mühendisler, kendi kopyalarını oluşturabilen robotlar geliştirdi." },
    new { Title = "Akıllı Şehirler Kendi Kendini Yönetecek", Description = "Geliştirilen yeni yapay zeka sistemi, akıllı şehirlerde trafik, enerji ve su yönetimini tamamen otonom olarak kontrol edecek. Bu sistem, şehir hayatını daha sürdürülebilir ve verimli hale getirmeyi hedefliyor." },
    new { Title = "Yapay Zeka Destekli Çiftlikler Rekor Ürün Verimi Sağladı", Description = "Robotik tarım araçları ve yapay zeka destekli sulama sistemleri sayesinde çiftlikler, geleneksel yöntemlere göre üç kat daha fazla ürün elde etti. Tarım sektörü büyük bir dönüşüm yaşıyor." },
    new { Title = "Dünya'nın En Derin Mağarasında Gizli Bir Ekosistem Bulundu", Description = "Bilim insanları, Dünya'nın en derin mağarasında daha önce bilinmeyen bitki ve hayvan türlerinin oluşturduğu izole bir ekosistem keşfetti. Bu buluş, yaşamın sınırlarını yeniden tanımlıyor." },
    new { Title = "Antarktika'da Milyonlarca Yıllık Donmuş Orman Keşfedildi", Description = "Antarktika'da yapılan kazılarda, buzulların altında milyonlarca yıl önce var olmuş donmuş bir orman keşfedildi. Araştırmacılar, bu keşfin iklim değişikliğiyle ilgili önemli ipuçları sunduğunu belirtiyor." },
    new { Title = "Dünyanın İlk Tamamen Geri Dönüşümlü Arabası Tanıtıldı", Description = "Otomotiv sektörü, tamamen geri dönüşümlü malzemelerden üretilmiş ve sıfır emisyon salınımı yapan bir araç geliştirdi. Bu araç, çevre dostu otomobil teknolojilerinde bir devrim yaratabilir." },
    new { Title = "Uzaydaki Gizemli Sinyallerin Kaynağı Bulundu", Description = "Astronomlar, yıllardır uzaydan gelen gizemli radyo sinyallerinin kaynağını keşfetti. Sinyallerin, galaksimizin dışındaki bir nötron yıldızından geldiği ortaya çıktı." },
    new { Title = "İnsan Beyni Bilgisayarlara Bağlanarak Veri Depolamaya Başladı", Description = "Yeni bir teknoloji sayesinde insan beyinleri, bilgisayarlar gibi veri depolama cihazlarına bağlanabiliyor. Bu buluş, hafıza kaybı yaşayan hastalar için umut ışığı olabilir." },
    new { Title = "Okyanus Tabanında Kaybolan Uygarlığın Kalıntıları Bulundu", Description = "Deniz araştırmacıları, okyanus tabanında kaybolmuş antik bir uygarlığın kalıntılarına rastladı. Buluntular, tarih kitaplarının yeniden yazılmasına yol açabilir." },
    new { Title = "İnsanlar Artık Rüya Gördükleri Anları Kaydedebiliyor", Description = "Yeni geliştirilen bir cihaz, insanların gördükleri rüyaları video formatında kaydetmeyi sağlıyor. Bilim insanları, bu cihazla rüya analizinde çığır açmayı planlıyor." },
    new { Title = "Yerçekimsiz Spor Salonları Popülerleşmeye Başladı", Description = "Bir spor teknolojisi şirketi, yerçekimsiz ortamda egzersiz yapılmasını sağlayan özel spor salonları geliştirdi. Bu yöntem, kas gelişimini daha hızlı hale getiriyor." },
    new { Title = "Bitkiler Müzik Dinleyerek Daha Hızlı Büyüyor", Description = "Yapılan deneylerde, bitkilere belirli frekansta müzik çalındığında büyüme hızlarının iki kat arttığı gözlemlendi. Araştırmacılar, bu yöntemin tarımda devrim yaratacağını düşünüyor." },
    new { Title = "Deniz Yüzeyinde Enerji Üreten Yapay Adalar Kuruldu", Description = "Mühendisler, deniz yüzeyinde güneş ve rüzgar enerjisi üreten yapay adalar inşa etti. Bu adalar, enerji ihtiyacını karşılamada önemli bir çözüm sunuyor." },
    new { Title = "Zamanda Yolculuk Simülasyonu Gerçek Oldu", Description = "Yeni bir sanal gerçeklik platformu, insanlara geçmişe ve geleceğe yolculuk yapıyormuş hissi veren bir zaman yolculuğu simülasyonu geliştirdi." },
    new { Title = "Kendi Kendini Temizleyen Şehirler İnşa Ediliyor", Description = "Akıllı malzemelerle donatılmış şehirler, hava ve su kirliliğini otomatik olarak temizleyerek sürdürülebilir yaşam alanları oluşturuyor." },
    new { Title = "Ay'da İnşa Edilen İlk Otel Ziyaretçilere Kapılarını Açıyor", Description = "Uzay turizmi kapsamında Ay yüzeyinde inşa edilen ilk lüks otel, turistleri ağırlamaya hazırlanıyor. Rezervasyonlar şimdiden dolmaya başladı." },
    new { Title = "Bitki Köklerinden Enerji Üreten Cihaz Geliştirildi", Description = "Araştırmacılar, bitki köklerinin doğal elektriğini kullanarak enerji üreten bir cihaz geliştirdi. Bu teknoloji, doğa dostu enerji üretiminde devrim yaratıyor." },
    new { Title = "Okyanus Altında Devasa Bir Kristal Mağara Bulundu", Description = "Dalgıçlar, okyanus tabanında içi devasa kristallerle kaplı büyüleyici bir mağara keşfetti. Bu mağara, jeologlar için büyük bir ilgi odağı oldu." },
    new { Title = "İnsanlar Artık Düşünceleriyle Yemek Pişirebiliyor", Description = "Yapay zeka ve düşünce gücüyle çalışan yeni bir mutfak cihazı, yemek tariflerini zihinsel komutlarla pişirebiliyor." },
    new { Title = "Mars'a Gönderilen Tohumlar İlk Meyvelerini Verdi", Description = "Mars'ta kolonileşme çalışmaları kapsamında gönderilen bitki tohumları ilk meyvelerini verdi. Bilim insanları, bu başarının insanlık için büyük bir adım olduğunu belirtiyor." },
    new { Title = "Sanal Gerçeklikte Futbol Maçları Gerçek Gibi Yaşanıyor", Description = "Yeni geliştirilen VR teknolojisi, kullanıcıların futbol maçlarını gerçek bir oyuncu gibi sahada yaşamasını sağlıyor. Oyuncular sanal ortamda koşuyor, topa vuruyor ve gerçek zamanlı olarak deneyim yaşıyor." },
    new { Title = "Maraton Koşucuları için Enerji Üreten Ayakkabı Tanıtıldı", Description = "Yeni nesil akıllı ayakkabılar, koşucuların adımlarını enerjiye dönüştürerek telefon ve saat gibi cihazların şarj edilmesini sağlıyor. Bu teknoloji, uzun mesafe koşucularını hedefliyor." },
    new { Title = "Basketbolculara Özel Yapay Zeka Destekli Antrenman Sistemi Geliştirildi", Description = "Yapay zeka destekli yeni bir sistem, basketbolcuların şut, pas ve hareket analizlerini değerlendirerek performanslarını artırmaları için özel antrenman programları oluşturuyor." },
    new { Title = "Futbol Topları Oyuncuların Sağlık Durumunu İzleyecek", Description = "Geliştirilen akıllı futbol topları, oyuncuların kalp ritmi ve nabız gibi sağlık verilerini ölçerek antrenman sırasında anlık bilgi sağlıyor. Bu toplar sayesinde sakatlanmaların önüne geçilmesi hedefleniyor." },
    new { Title = "Robot Hakemler İlk Kez Bir Futbol Turnuvasında Görev Yaptı", Description = "Yapay zeka destekli robot hakemler, insan hakemlerin yerine geçerek futbol maçlarında karar verme süreçlerini hızlandırdı. Bu sistemin, hata oranını büyük ölçüde azalttığı belirtildi." },
    new { Title = "Olimpiyatlarda Uçan Kaykay Yarışı Heyecan Yarattı", Description = "Yeni bir ekstrem spor dalı olarak uçan kaykay yarışları Olimpiyatlarda tanıtıldı. Sporcular, havada akrobatik hareketler yaparak izleyicilere görsel bir şölen sundu." },
    new { Title = "E-Spor Oyuncularına Fiziksel Antrenman Programı Zorunlu Hale Getirildi", Description = "E-spor turnuvalarına katılan oyuncular için fiziksel dayanıklılığı artırmaya yönelik antrenman programları zorunlu hale getirildi. Böylece oyuncuların zihinsel ve fiziksel performansı artırılmak isteniyor." },
    new { Title = "Yapay Zeka, Futbol Maçlarında Taktiksel Kararları Yönlendiriyor", Description = "Futbol kulüpleri, yapay zeka analizleriyle maç sırasında anlık taktiksel kararlar alarak sahadaki performansı optimize ediyor. Bu sistemle teknik direktörlerin iş yükü azaltılıyor." },
    new { Title = "100 Metre Koşusunda Yeni Dünya Rekoru: Robot İnsandan Hızlı Koştu", Description = "Bir teknoloji firması tarafından geliştirilen robot, 100 metre koşusunu 8 saniyenin altında tamamlayarak insan rekorunu kırdı. Bu robotun atletizmde yeni bir dönemi başlatacağı söyleniyor." },
    new { Title = "Dalış Sporunda Derinlik Rekoru Kıran Yeni Ekipman Tanıtıldı", Description = "Dalgıçlar için geliştirilen yeni ekipmanlar, oksijen tüketimini optimize ederek çok daha derinlere inme imkanı sağlıyor. Bu ekipmanlar, dalış sporunu daha güvenli hale getiriyor." },
    new { Title = "Formula 1 Araçları için Sürücüsüz Yarış Dönemi Başlıyor", Description = "Otonom araç teknolojisi, Formula 1 yarışlarına da uyarlanarak sürücüsüz araçların yarıştığı yeni bir lig başlatıldı. Bu yarışlar, mühendislik becerilerinin sınırlarını zorlamayı amaçlıyor." },
    new { Title = "Yüzücüler için Su Altında Anlık Performans Takibi Sağlayan Gözlük Geliştirildi", Description = "Akıllı yüzme gözlükleri, yüzücülerin hız, kulaç sayısı ve nefes ritmini su altında anlık olarak ölçüp ekrana yansıtıyor. Bu teknoloji, yüzme antrenmanlarını daha verimli hale getiriyor." },
    new { Title = "Çim Hokeyi Maçlarında Drone Hakemler Görev Alıyor", Description = "Hokey maçlarında kullanılmaya başlanan drone hakemler, saha üzerindeki tüm açılardan görüntü alarak kararların doğru verilmesini sağlıyor. Hata payı minimuma indirildi." },
    new { Title = "Yapay Zeka Destekli Tenis Raketleri Oyunculara Özel Tavsiyeler Veriyor", Description = "Akıllı tenis raketleri, oyuncuların vuruş gücünü, açılarını ve tekniklerini analiz ederek antrenman sonrası performans iyileştirme önerileri sunuyor." },
    new { Title = "Kış Sporlarında Hava Şartlarına Dayanıklı Akıllı Kıyafetler Tanıtıldı", Description = "Kayak ve snowboard sporcuları için geliştirilen akıllı kıyafetler, vücut ısısını otomatik olarak ayarlayarak ekstrem hava şartlarında maksimum konfor sağlıyor." },
    new { Title = "Yeni Nesil Golf Topları Mesafeyi Kendileri Hesaplıyor", Description = "Golf oyuncuları için üretilen akıllı toplar, atış mesafesini ve açısını analiz ederek oyunculara ideal vuruş önerileri sunuyor. Bu toplar, performansı artırmayı hedefliyor." },
    new { Title = "Rüzgar Sörfü için Kendi Kendini Dengeleyen Tahtalar Geliştirildi", Description = "Yeni geliştirilen akıllı sörf tahtaları, rüzgar sörfü yapanların dengesini otomatik olarak koruyarak sporun öğrenilmesini kolaylaştırıyor ve daha güvenli hale getiriyor." },
    new { Title = "Futbol Sahalarında Enerji Üreten Yapay Çim Kullanılıyor", Description = "Yeni nesil yapay çimler, futbolcuların adımlarından enerji üreterek statlardaki elektrik ihtiyacını karşılamaya yardımcı oluyor. Bu sistem, sürdürülebilir spor tesisleri için umut vadediyor." },
    new { Title = "Buz Hokeyinde Görünmez Kasklar Sporculara Yeni Bir Deneyim Sunuyor", Description = "Şeffaf malzemeden yapılan yeni nesil buz hokeyi kaskları, sporculara daha geniş bir görüş açısı sağlayarak performanslarını artırıyor." },
    new { Title = "Kripto Parada Yeni Dönem: Dijital Altın Çağı Başlıyor", Description = "Uzmanlar, Bitcoin ve Ethereum gibi kripto paraların değer kazanmaya devam edeceğini ve dijital altının geleceğin yatırım aracı olacağını belirtiyor. Ekonomistler, bu yükselişin global piyasalarda önemli değişimlere yol açacağını söylüyor." },
    new { Title = "Merkez Bankaları Dijital Para Birimlerini Test Etmeye Başladı", Description = "Birçok ülkenin merkez bankası, kendi dijital para birimlerini piyasaya sürerek geleneksel paraya alternatif yaratmak için çalışmalarını hızlandırdı. Dijital para birimleri, finansal işlemlerin daha hızlı ve güvenli hale gelmesini sağlıyor." },
    new { Title = "Dünya Ekonomisinde Yapay Zeka Destekli Tahminler Yükselişte", Description = "Yapay zeka, global ekonomideki büyüme oranlarını tahmin ederek finansal yatırımcılara ışık tutuyor. Bu teknoloji sayesinde daha doğru ekonomik analizler yapılabiliyor ve riskler minimize edilebiliyor." },
    new { Title = "Evden Çalışma Ekonomisi Büyüyor: Şirketler Ofisleri Terk Ediyor", Description = "Pandemi sonrası evden çalışma modeli kalıcı hale gelirken, birçok büyük şirket fiziksel ofislerden vazgeçerek maliyetleri azaltma yoluna gidiyor. Bu trend, gayrimenkul sektöründe önemli değişikliklere neden oluyor." },
    new { Title = "Küresel Enflasyonla Mücadelede Yeni Stratejiler Geliştiriliyor", Description = "Dünya genelindeki yüksek enflasyon oranlarına karşı ülkeler, faiz artışları ve maliye politikaları ile mücadele etmeye çalışıyor. Ekonomistler, bu dönemde tasarrufun ve akıllı yatırımların önemine dikkat çekiyor." },
    new { Title = "E-Ticaret Sektörü Pandemi Sonrası Rekor Büyüme Kaydediyor", Description = "Online alışveriş platformları, pandemi sonrası dönemde rekor gelirler elde ederek ekonomi içinde önemli bir paya ulaştı. Küçük işletmeler de e-ticaret sayesinde global pazarlara erişim sağlıyor." },
    new { Title = "Enerji Krizi Çözüldü: Yenilenebilir Kaynaklara Dev Yatırım", Description = "Gelişmiş ülkeler, enerji krizine çözüm olarak güneş ve rüzgar enerjisine milyarlarca dolar yatırım yapıyor. Bu dönüşüm, hem ekonomiyi canlandırıyor hem de çevre dostu bir enerji geleceği oluşturuyor." },
    new { Title = "Borsa Yatırımcıları Yapay Zeka ile Hızlı Karar Alıyor", Description = "Borsada işlem yapan yatırımcılar, yapay zeka algoritmaları sayesinde saniyeler içinde kararlar alarak büyük kazançlar elde ediyor. Bu teknoloji, finans dünyasında devrim niteliğinde bir değişim yaratıyor." },
    new { Title = "Altın Fiyatlarında Tarihi Zirve: Yatırımcılar Altına Yöneliyor", Description = "Son dönemde altın fiyatları rekor seviyelere ulaşırken, yatırımcılar güvenli liman olarak altına yöneliyor. Ekonomistler, bu yükselişin devam edeceğini öngörüyor." },
    new { Title = "Startuplar Ekonomiyi Canlandırıyor: Yatırımlar Hız Kazandı", Description = "Teknoloji ve yenilikçi fikirlerle öne çıkan startuplar, yatırımcıların gözdesi haline geldi. Bu şirketler, ekonomiye katkı sağlayarak yeni istihdam alanları yaratıyor." },
    new { Title = "Dijital Bankacılıkta Rekabet Kızışıyor: Müşteri Memnuniyeti Odakta", Description = "Dijital bankalar, hızlı hizmet ve düşük maliyet avantajlarıyla geleneksel bankaların pazar payını azaltmaya başladı. Rekabetin artması, müşterilere daha iyi hizmet seçenekleri sunuyor." },
    new { Title = "Lüks Tüketim Pazarı Krize Rağmen Büyümeye Devam Ediyor", Description = "Küresel ekonomik belirsizliklere rağmen lüks tüketim ürünlerine olan talep artmaya devam ediyor. Bu segmentteki büyüme, lüks markaların satışlarını rekor seviyelere taşıdı." },
    new { Title = "Blockchain Teknolojisi Ticarette Güvenliği Artırıyor", Description = "Blockchain, uluslararası ticarette işlem güvenliğini ve şeffaflığı artırarak dolandırıcılığı engelliyor. Bu teknoloji, özellikle lojistik ve tedarik zinciri yönetiminde devrim yaratıyor." },
    new { Title = "Ekonomide Döngüsel Model: Sürdürülebilir Üretim Trendi Yükseliyor", Description = "Şirketler, atıkları yeniden değerlendirerek üretim maliyetlerini düşürüyor ve sürdürülebilir bir ekonomi modeli oluşturuyor. Bu model, çevreyi korurken şirketlerin kârlılığını artırıyor." },
    new { Title = "Küçük İşletmelere Dijitalleşme Desteği Ekonomiyi Büyütüyor", Description = "Devlet destekleri ve teknoloji platformları, küçük işletmelerin dijitalleşmesini hızlandırarak ekonomik büyümeye katkıda bulunuyor. Dijital araçlar sayesinde KOBİ'ler global pazarlarda rekabet edebiliyor." },
    new { Title = "Yatırımcılar Yeşil Tahvillere Yöneliyor: Çevre Dostu Ekonomi", Description = "Çevreye duyarlı projeleri finanse eden yeşil tahviller, yatırımcıların ilgisini çekmeye başladı. Bu tahviller, sürdürülebilir kalkınma hedeflerine ulaşmak için önemli bir araç olarak görülüyor." },
    new { Title = "Yeni Finansal Araçlar Küresel Krizlere Karşı Koruma Sağlıyor", Description = "Ekonomistler, yeni finansal enstrümanların küresel ekonomik krizlerin etkilerini azaltarak piyasaları daha dayanıklı hale getireceğini belirtiyor. Risk yönetimi teknolojileri bu dönemde öne çıkıyor." },
    new { Title = "Gayrimenkul Sektöründe Yapay Zeka ile Değerleme Dönemi", Description = "Gayrimenkul piyasasında yapay zeka destekli değerleme sistemleri, alım-satım süreçlerini hızlandırarak güvenilir fiyat analizleri sunuyor. Bu teknoloji, yatırımcıların karar süreçlerini kolaylaştırıyor." },
    new { Title = "Küresel Ekonomide Nakitsiz Toplum Dönemi Başlıyor", Description = "Birçok ülke, nakitsiz ödeme sistemlerine geçiş yaparak finansal işlemleri hızlandırmayı ve kayıt dışı ekonomiyi azaltmayı hedefliyor. Dijital ödemeler, günlük yaşamın bir parçası haline geliyor." },
    new { Title = "2024 İlkbahar/Yaz Moda Trendleri Belli Oldu", Description = "Ünlü tasarımcılar, 2024 ilkbahar/yaz koleksiyonlarını tanıtarak moda dünyasında yeni akımları ortaya çıkardı. Canlı renkler, çiçek desenleri ve retro kesimler bu sezonun öne çıkan detayları arasında yer alıyor." },
    new { Title = "Sokak Modasında Oversize Rüzgarı Esiyor", Description = "Son yıllarda popülerliğini artıran oversize giysiler, sokak modasının en gözde parçaları haline geldi. Geniş kesimler ve rahat tarzlar, gençlerin favorisi olmaya devam ediyor." },
    new { Title = "Vegan Deri Ayakkabılar Moda Dünyasını Sallıyor", Description = "Çevre dostu ve sürdürülebilir moda akımı, vegan deri ayakkabıları öne çıkarıyor. Ünlü markalar bu yeni trendi benimseyerek doğa dostu koleksiyonlarını tüketicilere sunuyor." },
    new { Title = "Paris Moda Haftası'nda Sıra Dışı Tasarımlar Göz Kamaştırdı", Description = "Paris Moda Haftası, bu yıl da göz alıcı ve yenilikçi tasarımlara ev sahipliği yaptı. Ünlü markaların podyuma taşıdığı avant-garde parçalar, moda severlerin ilgisini topladı." },
    new { Title = "Minimalist Tarz, Gardıropların Vazgeçilmezi Oluyor", Description = "Abartıdan uzak, sade ve şık parçalar moda dünyasında minimalist akımı ön plana çıkarıyor. Kaliteli kumaşlar ve zamansız tasarımlar, her sezon trend olmaya devam ediyor." },
    new { Title = "Moda Markaları Sürdürülebilir Koleksiyonlara Yöneliyor", Description = "Çevresel etkileri azaltmayı amaçlayan moda markaları, geri dönüştürülebilir malzemelerle sürdürülebilir koleksiyonlar hazırlayarak çevre bilincine katkı sağlıyor." },
    new { Title = "Sneaker Çılgınlığı: Rahatlık ve Şıklık Bir Arada", Description = "Hem spor hem de şık kombinlerin tamamlayıcısı olan sneaker modelleri, her yaş grubunun favori ayakkabısı olmaya devam ediyor. Ünlü markaların yeni koleksiyonları büyük ilgi görüyor." },
    new { Title = "Retro Modası Geri Döndü: 70'ler ve 90'lar Yeniden Sahne Alıyor", Description = "70'ler ve 90'ların ikonik parçaları, bu yılın moda trendlerine yön veriyor. Geniş paçalı pantolonlar, denim ceketler ve vintage aksesuarlar tekrar popüler hale geldi." },
    new { Title = "Lüks Markaların Sokak Modası ile Buluşması: 'Athleisure' Trend", Description = "Spor giyim ile günlük tarzı birleştiren 'athleisure' trendi, lüks markaların koleksiyonlarında sıkça yer alıyor. Hem rahat hem şık kombinler, şehir hayatının yeni favorisi." },
    new { Title = "Kış Sezonunda Yün ve Kaşmir Geri Dönüyor", Description = "Kış sezonunda sıcak ve şık kalmak isteyenler için yün ve kaşmir parçalar yeniden moda oldu. Kaliteli dokuları ve zarif tasarımlarıyla dikkat çeken bu parçalar, gardıropların gözdesi." },
    new { Title = "Unisex Moda: Cinsiyetsiz Tasarımlar Trendlere Yön Veriyor", Description = "Moda dünyasında cinsiyet kalıplarını yıkan unisex tasarımlar, genç nesil tarafından büyük ilgi görüyor. Sade ve rahat parçalar, herkesin gardırobunda kendine yer buluyor." },
    new { Title = "Ekolojik Moda Hareketi: Doğa Dostu Kumaşlar Öne Çıkıyor", Description = "Organik pamuk, bambu ve geri dönüştürülmüş malzemelerden üretilen kıyafetler, ekolojik moda hareketinin bir parçası olarak büyük ilgi görüyor. Çevre bilincine sahip tüketiciler bu akıma yöneliyor." },
    new { Title = "Teknoloji ve Moda Birleşiyor: Akıllı Giysiler Çağı", Description = "Akıllı kumaşlar ve giyilebilir teknoloji, moda dünyasında yeni bir dönem başlatıyor. Vücut sıcaklığını dengeleyen, enerji üreten ve sağlık izleme özelliklerine sahip giysiler geliştiriliyor." },
    new { Title = "Moda Dünyasında Renk Patlaması: Neon Tonları Geri Döndü", Description = "Cesur ve dikkat çekici görünmek isteyenler için neon tonları geri döndü. Bu sezon özellikle neon yeşil, pembe ve turuncu tonları, koleksiyonlarda öne çıkan renkler arasında yer alıyor." },
    new { Title = "İkinci El Moda Trendi: Vintage Parçalar Altın Çağını Yaşıyor", Description = "Sürdürülebilir modanın bir parçası olan ikinci el kıyafetler ve vintage parçalar, yeniden popüler hale geldi. Bu trend, hem ekonomik hem de çevre dostu bir seçenek olarak öne çıkıyor." },
    new { Title = "Aksesuar Trendleri: Büyük Çantalar Geri Geliyor", Description = "Küçük çantaların yerini alan büyük ve fonksiyonel çantalar, bu sezonun favori aksesuarları arasında yer alıyor. Hem şık hem de kullanışlı tasarımlar dikkat çekiyor." },
    new { Title = "Dijital Moda Defileleri: Geleceğin Tanıtım Platformu", Description = "Teknolojiyle entegre olan moda dünyası, dijital defileler aracılığıyla daha geniş kitlelere ulaşıyor. Sanal ortamda düzenlenen defileler, izleyicilere benzersiz bir deneyim sunuyor." },
    new { Title = "Ayakkabı Modasında Platform Topuklar Yükselişte", Description = "Rahatlığı ve şıklığı bir arada sunan platform topuklu ayakkabılar, bu sezonun gözdesi oldu. Özellikle gece davetlerinde bu modeller sıkça tercih ediliyor." },
    new { Title = "Yılın Moda İkonu: Sade ve Şık Stilleriyle Öne Çıkan Ünlüler", Description = "Ünlü isimler, sade ama zarif tarzlarıyla bu yılın moda ikonları arasında gösteriliyor. Sokak stilinden kırmızı halıya kadar geniş bir yelpazede ilham verici kombinler yaratıyorlar." },
};

foreach (var news in fakeNews)
    csvFile += $"{id++},{news.Title.Replace(',', ';')},{news.Description.Replace(',', ';')},1\n";

File.WriteAllText(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "Dataset.csv"), csvFile);
