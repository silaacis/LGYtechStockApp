# 📦 LGYtech Stok ve Sipariş Uygulaması

Küçük bir toptancı için geliştirilmiş mini **stok ve sipariş yönetimi** uygulaması.
Ürünleri yönetebilir, birden fazla satırlı sipariş oluşturabilir ve stok hareketlerini takip edebilirsiniz.

![.NET Framework](https://img.shields.io/badge/.NET%20Framework-4.8-512BD4?logo=dotnet&logoColor=white)
![ASP.NET MVC](https://img.shields.io/badge/ASP.NET%20MVC-5-5C2D91)
![Entity Framework](https://img.shields.io/badge/Entity%20Framework-6-2D8CFF)
![SQL Server](https://img.shields.io/badge/SQL%20Server-Express-CC2927?logo=microsoftsqlserver&logoColor=white)
![Bootstrap](https://img.shields.io/badge/Bootstrap-jQuery-7952B3?logo=bootstrap&logoColor=white)

## 📑 İçindekiler

- [Özellikler](#-özellikler)
- [Kullanılan Teknolojiler](#-kullanılan-teknolojiler)
- [Sayfalar](#-sayfalar)
- [Gereksinimler](#-gereksinimler)
- [Kurulum](#-kurulum)
- [Başlangıç Verileri](#-başlangıç-verileri)
- [Kabul Senaryoları](#-kabul-senaryoları)
- [Bonus: Eşzamanlı Stok Kontrolü](#-bonus-eşzamanlı-stok-kontrolü)
- [Güvenlik ve Yapılandırma Notları](#-güvenlik-ve-yapılandırma-notları)
- [Bilinen Kapsam](#-bilinen-kapsam)

---

## ✨ Özellikler

**Ürün yönetimi**
- Ürünleri listeleme, **ada göre arama** ve **sunucu tarafında sayfalama** (sayfa başı 10 kayıt)
- Ürün ekleme ve düzenleme
- Ürün adı (zorunlu), fiyat (> 0), stok (≥ 0) ve benzersiz SKU doğrulamaları; hatalar ilgili alanın yanında gösterilir

**Sipariş yönetimi**
- Müşteri adı ve birden fazla ürün satırı içeren sipariş oluşturma
- Satır ekleme ve silme işlemleri sayfa yenilenmeden yapılır
- "Siparişi Ver" işlemi **jQuery AJAX POST** ile gönderilir; başarıda sipariş numarası, hatada stok yetersizliği mesajı gösterilir
- Sipariş listesi: satır sayısı ve toplam tutar ile birlikte

**Stok ve veri bütünlüğü**
- Sipariş, sipariş satırları, stok düşümü ve stok hareketleri **tek bir SQL Server transaction'ı** içinde kaydedilir
- Bir üründe bile stok yetmezse **hiçbir kayıt oluşturulmaz**
- Ürün bazında stok hareketi geçmişi
- Eşzamanlı siparişlerde stoğun negatif olmasını önleyen **koşullu stok güncellemesi**

**Güvenlik**
- AJAX POST istekleri **Anti-Forgery Token** ile korunur

---

## 🛠 Kullanılan Teknolojiler

| Katman | Teknoloji |
| --- | --- |
| Framework | .NET Framework 4.8, ASP.NET MVC 5 |
| ORM | Entity Framework 6 (Code First Migrations) |
| Veritabanı | SQL Server Express |
| Arayüz | Razor, jQuery, Bootstrap |
| İstek güvenliği | AJAX, Anti-Forgery Token |

---

## 🧭 Sayfalar

| Sayfa | Adres |
| --- | --- |
| Ana sayfa | `/` |
| Ürün listesi | `/Products/ProductList` |
| Yeni ürün | `/Products/AddProduct` |
| Ürün düzenleme | `/Products/EditProduct/{id}` |
| Sipariş listesi | `/Orders/OrderList` |
| Yeni sipariş | `/Orders/CreateOrder` |
| Stok hareketleri | `/StockMovements` |

---

## 📋 Gereksinimler

- Windows
- Visual Studio 2022
- .NET Framework 4.8 Developer Pack
- SQL Server Express
- SQL Server Management Studio (SSMS)
- NuGet paketlerini geri yüklemek için internet bağlantısı

---

## 🚀 Kurulum

### 1. Repoyu indirin

Repoyu klonlayın veya GitHub üzerinden ZIP olarak indirin. Solution (`.sln`) dosyasını Visual Studio'da açın.

### 2. Connection string'i düzenleyin

`Web.config` dosyasındaki `connectionStrings` bölümünü açın ve bağlantı dizesini kendi SQL Server Express sunucu adınıza göre düzenleyin.

```xml
<connectionStrings>
  <add name="StockDbContext"
       connectionString="Data Source=LOQ-sila\SQLEXPRESS;Initial Catalog=LGYtechStockDb;Integrated Security=True;MultipleActiveResultSets=True"
       providerName="System.Data.SqlClient" />
</connectionStrings>
```

> `LOQ-sila\SQLEXPRESS` örnek sunucu adıdır. Kendi bilgisayarınızdaki SQL Server instance adını kullanın.
> Projedeki `DbContext` için kullanılan connection string adının `StockDbContext` olduğundan emin olun; farklıysa mevcut adı koruyun.

### 3. NuGet paketlerini geri yükleyin

Visual Studio'da solution'a sağ tıklayıp **Restore NuGet Packages** seçeneğini kullanın.

### 4. Veritabanını oluşturun

**Tools → NuGet Package Manager → Package Manager Console** bölümünü açın. *Default project* olarak Entity Framework migration'larının bulunduğu projeyi seçin ve çalıştırın:

```powershell
Update-Database
```

Migration'lar ve seed işlemi çalıştıktan sonra veritabanı oluşturulur ve başlangıç ürünleri eklenir.

> Komut hata verirse *Startup project*, *Default project* ve `Web.config` connection string ayarlarını kontrol edin.

### 5. Uygulamayı çalıştırın

Web projesini *Startup project* olarak seçin ve **F5** ile çalıştırın. Uygulamanın açtığı yerel adres üzerinden ürün, sipariş ve stok hareketleri sayfalarına gidebilirsiniz.

---

## 🌱 Başlangıç Verileri

Seed işlemiyle aşağıdaki ürünler eklenir:

| Ürün | Başlangıç stoğu |
| --- | :---: |
| Kalem | 100 |
| Defter | 50 |
| Silgi | 3 |
| Cetvel | 1 |
| Çanta | 0 |

Ürün fiyatları ve SKU değerleri uygulamadaki seed tanımına göre belirlenmiştir.

---

## ✅ Kabul Senaryoları

> Senaryoları birbirinden bağımsız yürütün. Her senaryodan önce gerekiyorsa stokları başlangıç değerlerine döndürün. Sipariş numaraları veritabanındaki mevcut kayıtlara göre değişebilir.

**Kontrol ve sıfırlama için yardımcı sorgular (SSMS):**

```sql
-- Güncel stokları görmek için
SELECT Name, Stock FROM Products;

-- Son stok hareketlerini görmek için
SELECT TOP 10 * FROM StockMovements ORDER BY Id DESC;

-- Stokları başlangıç değerlerine döndürmek için
UPDATE Products SET Stock = 100 WHERE Name = N'Kalem';
UPDATE Products SET Stock = 50  WHERE Name = N'Defter';
UPDATE Products SET Stock = 3   WHERE Name = N'Silgi';
UPDATE Products SET Stock = 1   WHERE Name = N'Cetvel';
UPDATE Products SET Stock = 0   WHERE Name = N'Çanta';
```

### Senaryo A — Başarılı sipariş

**Adımlar**
1. Kalem stoğunun **100**, Defter stoğunun **50** olduğundan emin olun.
2. Sipariş oluşturma sayfasını açın.
3. Müşteri adı girin.
4. Kalem için adet **2**, Defter için adet **5** olan iki satır ekleyin.
5. **Siparişi Ver** butonuna tıklayın.

**Beklenen sonuç**
- Sipariş başarıyla oluşturulur ve sipariş numarası gösterilir.
- Kalem stoğu **98**, Defter stoğu **45** olur.
- İlgili sipariş için `StockMovements` tablosuna iki kayıt eklenir: Kalem için `-2`, Defter için `-5`.

### Senaryo B — Stok yetersizliği

**Adımlar**
1. Kalem stoğunun **100**, Silgi stoğunun **3** olduğundan emin olun.
2. Yeni bir siparişte Kalem için adet **2**, Silgi için adet **5** girin.
3. **Siparişi Ver** butonuna tıklayın.

**Beklenen sonuç**
- Sipariş reddedilir ve stok yetersizliği mesajı gösterilir.
- Kalem stoğu değişmeden **100** kalır.
- Bu başarısız deneme için `Orders`, `OrderLines` ve `StockMovements` tablolarına yeni kayıt eklenmez (transaction geri alınır).

> Senaryoyu tekrar etmeden önce stokları ve veritabanındaki kayıtları kontrol edin.

### Senaryo C — Anti-Forgery Token olmadan istek

**Adımlar**
1. Sipariş oluşturma sayfasını açın ve tarayıcı geliştirici araçlarını (F12) açın.
2. **Console** sekmesinde, `__RequestVerificationToken` alanı olmadan bir istek gönderin. Örnek (geçerli bir ürün Id'si kullanın):

```js
$.post('/Orders/CreateOrder', {
    CustomerName: 'Token Testi',
    'OrderLines[0].ProductId': 1,
    'OrderLines[0].Quantity': 1
}).fail(function (xhr) { console.log('Durum kodu:', xhr.status); });
```

**Beklenen sonuç**
- Sunucu isteği reddeder.
- Sipariş oluşturulmaz ve veritabanına sipariş kayıtları eklenmez.

> Bu testi yalnızca yerel geliştirme ortamında yapın. Normal kullanımda Anti-Forgery Token alanını kaldırmayın.

---

## 🏁 Bonus: Eşzamanlı Stok Kontrolü

İki kullanıcı aynı anda son Cetvel'i sipariş etmeye çalıştığında **yalnızca bir siparişin başarılı olması** gerekir.

### Nasıl çözüldü?

Stok azaltma işlemi, stok miktarını kontrol eden **koşullu bir SQL sorgusuyla** yapılır. Mantık şu şekildedir:

```sql
UPDATE Products
SET Stock = Stock - @Quantity
WHERE Id = @ProductId AND Stock >= @Quantity;
```

Etkilenen satır sayısı **0** ise stok yetersiz (veya aynı anda başka bir sipariş tarafından tüketilmiş) demektir. Bu durumda transaction **geri alınır** ve tutarsız kayıt oluşmaz.

### Test sonucu

Cetvel stoğu **1** olarak ayarlandı. Yapay zekâdan yardım alınarak hazırlanan `test_concurrency.py` betiği, iki farklı müşteri adına sipariş isteğini aynı anda gönderdi:

```bash
py test_concurrency.py
```

Alınan çıktı:

```text
('Test A', 200, '{"success":false,"message":"Stok sipariş sırasında değişti. Lütfen güncel stokla tekrar deneyin."}')
('Test B', 200, '{"success":true,"message":"Sipariş başarıyla oluşturuldu.","orderId":4}')
```

| İstek | Sonuç |
| --- | --- |
| Test A | ❌ Reddedildi (stok sipariş sırasında değişti) |
| Test B | ✅ Başarılı (sipariş no: 4) |

Son Cetvel için **yalnızca bir sipariş** başarılı oldu. Her iki istek de HTTP 200 döndüğü için siparişin başarılı olup olmadığı, durum koduna değil JSON yanıtındaki `success` alanına bakılarak değerlendirildi.

> Testi tekrar çalıştırmadan önce Cetvel stoğunu yeniden **1** yapın ve veritabanı durumunu kontrol edin. Yukarıdaki sipariş numarası ve çıktı bu çalıştırmaya aittir; sonraki çalıştırmalarda değişebilir.

---

## 🔐 Güvenlik ve Yapılandırma Notları

- Connection string ve yerel makineye özel ayarları kendi ortamınıza göre düzenleyin.
- Gerçek parola, token veya gizli bilgileri GitHub reposuna eklemeyin.
- Eşzamanlılık testi yerel geliştirme ortamı içindir. Test betiğinde yerel HTTPS geliştirme sertifikası nedeniyle sertifika doğrulaması kapatılmışsa bunu üretim ortamında kullanmayın.
- Siparişin başarılı olup olmadığı yalnızca HTTP durum koduna göre değil, JSON yanıtındaki `success` alanına göre de değerlendirilmelidir.

---

## 📌 Bilinen Kapsam

Bu README, uygulamada tamamlanmış özellikleri ve gerçekten çalıştırılan eşzamanlılık testini açıklar.

| Madde | Durum |
| --- | :---: |
| Ürün listesi (sayfalama + arama) | ✅ |
| Ürün ekleme / düzenleme ve doğrulamalar | ✅ |
| AJAX ile sipariş oluşturma | ✅ |
| Tek transaction ile sipariş kaydı | ✅ |
| Stok hareketleri ve sipariş listesi | ✅ |
| Anti-Forgery Token koruması | ✅ |
| Eşzamanlı stok kontrolü (bonus) | ✅ |
| T-SQL Stored Procedure (bonus) | ❌ Uygulanmadı |
