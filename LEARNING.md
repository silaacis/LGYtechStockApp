# 📚 LEARNING.md

Bu doküman, stok ve sipariş uygulamasını geliştirirken neler öğrendiğimi, hangi bölüme ne kadar zaman harcadığımı ve yapay zekâyı nerede kullandığımı anlatır.

## İçindekiler

1. [Bu görevden önce bilmediğim şeyler](#1-bu-görevden-önce-bilmediğim-şeyler)
2. [Bölümlere göre harcanan süre](#2-bölümlere-göre-harcanan-süre)
3. [Yapay zekâ kullanımı](#3-yapay-zekâ-kullanımı)

---

## 1. Bu görevden önce bilmediğim şeyler

### 1.1 Connection string'in `Web.config` içinde tanımlanması

Veritabanı bağlantı bilgisinin kod içine yazılmadan `Web.config` dosyasındaki `connectionStrings` bölümünde tutulduğunu öğrendim. `DbContext` bu bağlantıyı **ismiyle** bulduğu için `Web.config`'teki isim ile `DbContext` sınıfındaki isim aynı olmalı. Bu sayede kodu değiştirmeden farklı bir SQL Server'a bağlanabiliyorum.

**Nereden öğrendim:** Yapay zekâya sorarak öğrendim.

### 1.2 Dependency Injection (DI) kaydı için dışarıdan kütüphane kurma

.NET Framework 4.8 ve MVC 5'te, .NET Core'daki gibi hazır bir DI altyapısı olmadığını öğrendim. Bağımlılıkların (servisler, `DbContext` vb.) kaydı için **Autofac** gibi harici bir kütüphaneyi NuGet ile kurup yapılandırmak gerekiyor.

**Nereden öğrendim:** Yapay zekâya sorarak öğrendim.

### 1.3 Veritabanı transaction'ı (commit / rollback)

Daha önce yaptığım .NET projelerinde transaction kullanmamıştım. Bu görevde sipariş akışındaki tüm işlemleri tek bir transaction içinde yaptım:

- Sipariş (`Orders`) kaydı
- Sipariş satırları (`OrderLines`) kaydı
- Her ürün için stok düşümü
- Stok hareketi (`StockMovements`) kaydı

Bu adımlardan biri hata verirse (örneğin bir üründe stok yetersizse) işlem **rollback** ile tamamen geri alınıyor ve veritabanına hiçbir kayıt yazılmıyor. Hepsi sorunsuz tamamlanırsa **commit** ediliyor. Böylece "sipariş oluştu ama stok düşmedi" gibi tutarsız durumlar önleniyor.

**Nereden öğrendim:** Önce konuyu araştırdım ve mantığını öğrendim. Ardından kodu yazarken yapay zekâdan yardım aldım.

---

## 2. Bölümlere göre harcanan süre

| Bölüm | Harcanan süre |
| --- | :---: |
| Proje yapısı ve veritabanı modelleri | 2 saat |
| Ürün listeleme, arama ve sayfalama | 1 saat |
| Ürün ekleme, düzenleme ve doğrulama | 1 saat |
| Sipariş oluşturma ve dinamik form satırları | 2 saat |
| Transaction, stok kontrolü ve stok hareketleri | 2 saat |
| Sipariş listesi ve stok hareketleri arayüzü | 1 saat |
| Testler, hata ayıklama ve son kontroller | 2 saat |
| **Toplam** | **11 saat** |

> Süreler yaklaşık değerlerdir.

---

## 3. Yapay zekâ kullanımı

### 3.1 Yapay zekâyı nerede kullandım?

- **Yeni kavramları öğrenirken:** `Web.config` connection string yapısı ve Autofac ile DI kaydı gibi konuları anlamak için.
- **Transaction kodunu yazarken:** Konuyu önce kendim araştırdım, kod aşamasında yapay zekâdan yardım aldım.
- **Eşzamanlılık testi:** İki isteği aynı anda gönderen `test_concurrency.py` betiğini yapay zekâ yardımıyla hazırladım.
- **Arayüz tasarımı:** Sayfaların görünümünü (layout, tablolar, formlar, ana sayfa) modernleştirirken yapay zekâdan yardım aldım.
- **Dokümantasyon:** README'nin düzenlenmesinde yardım aldım.

### 3.2 Yapay zekâ beni nerede yanlış yönlendirdi?

Yazılan koda tam hâkim olmadığım yerlerde yapay zekâ özellikle **fonksiyon, metot ve controller isimleri** konusunda beni yanlış yönlendirebildi.

**Kodun projeye uyarlanması**
Yapay zekâ bazen mevcut proje yapısını ve bağımlılık kayıtlarını tam olarak dikkate almayan kodlar üretti. Bu yüzden her seferinde şunları kontrol etmem gerekti:
- `namespace` değerlerinin projedekiyle uyumu
- `DbContext` kullanımının mevcut yapıyla uyumu
- Mevcut proje düzeni ve bağımlılık kayıtlarıyla uyumluluk

**Yönlendirme (routing) ve action isimleri**
Örnek kodlardaki controller veya action isimleri projemdekilerle uyuşmadığında **404 hatası** aldım. Bu hatalardan şunu öğrendim: URL, controller adı ve action adı birlikte kontrol edilmeli; yapay zekânın verdiği isimler doğrudan kopyalanmamalı.

### 3.3 Çıkardığım ders

Yapay zekâ hızlı bir başlangıç sağlıyor, ancak ürettiği kodu **çalıştırmadan ve anlamadan** projeye eklemek hata kaynağı oluyor. Bu nedenle kodu satır satır okuyup projemdeki isimlerle karşılaştırdım ve çalıştırarak test ettim.
