# vuvy-merge

Mimari Yaklaşım

    -   Scoped-based mimari yaklaşım izlendi. GameLifetimeScope oyunun genel lifecycle'ını barındırmakta.

        - Grid scope ise grid sahnesine geçildiğinde run-time'da dinamik olarak oluşur. Ana menüye döndüldüğünde grid'i igilendiren her şey onunla birlikte dispose olur.

            - Bu sayede oluşturulan nesnelerin reset süreçleri, lifecycle yönetimi ve olası memory leak senaryoları merkezi ve kontrollü şekilde yönetilebilir.


    -   Grid Mimarisi
        
        - Grid sisteminin merkezi olarak yönetilebilmesi ve sürdürülebilirliğinin artırılması amacıyla, data ve view katmanları ayrıştırılarak MVC pattern uygulandı.
            
            - Tüm bu mimariyi kullanırken,
                - Data tarafında heap garbage collector baskısını minimumda tutmak adına struct type kullanıldı.
                - View katmanında, MonoBehaviour sınıflarına doğrudan bağımlılığı önlemek için interface tabanlı soyutlama kullanıldı.
                - Controller özelinde service'ler ile ISP'ye uygun gevşek bağlılık sağlandı
                


Tasarım Kalıpları

    -   Item Chain System(Data-Driven)
        - ItemChainData scriptable object ve kurulan sistem sayesinde eğer yeni bir item chain eklemek isterseniz kod tarafında sadece "ItemChainType" enum'a yeni type eklemek yeterli.
            - Sonrasında Unity'de oluşturulacak yeni ItemChainData'yı BoardItemConfig'e referansı iletildiğinde tüm sisteme yeni item chain eklenmiş olacaktır.(Potion sisteme dahil değil incelenebilir)


    -   EventBus(Observer Pattern)
        - Projede bulunan UI ile etkileşimlerin hepsi EventBus sistemi ile iletişim kuruyor. Bu sayede core sistem içerisine UI inject etmeyip daha gevşek bağlı sistemler kurmuş oluyoruz.


    -   Dependency Injection(DI)
        - VContainer ile hem bağımlıklıkları gevşek bağlı hem de aynı yaşam süresine sahip bağımlılıkları otomatik ve güvenli şekilde temizleniyor.


    -   Services
        - Drag -> Drop -> Merge 
            - Chain of Responsibility ile kurgulanmış GridController'dan başlayıp grid logic işlemlerini yapan service'ler zinciri. 
            - Her hamle yapıldığında birbirine bağlı olması gereken yapılar olduğundan bilinçli şekilde solid'e uygun şekilde düzenlendi.


    -   Factory Pattern
        - Item’ların oluşturulması, konfigüre edilmesi ve yönetilmesi süreçleri ItemFactory içerisinde toplanarak uygulandı.


    -   Object Pool
        - Grid sisteminde bulunan itemlar için oluşturuan sistem pool sayesinde Unity'de olan Instantiate-Destroy maliyetinden kazanç sağlanıyor.
    

Tercihler

    -   Input System
        - New Input System kullanılarak input yönetimi event-driven mimari kullanıldı. Bu sayede Update polling maliyetini ortadan kaldırarak daha modüler ve bakımı kolay bir yapı sağladı.


    -  Code Organization & Readability
        - Daha temiz ve okunabilir olması adına extensions(GridItemExtensions, GridCoordinateExtensions) ve Constants scriptleri oluşturuldu.
        - SlotPosition, GridSize gibi sadece x,y dimensions verileri tutan value type'lar için daha okunabilir olması adına bunları kapsayan struct'lar oluşturuldu.

    -   Sprite Atlas
        - Kullanılan görseller draw call optimizasyonu yapmak adına bir adet sprite atlas'ta bulunmakta.



Notlar 

    - Skor UI'da gözükmesi gerektiğinden, merge olduğunda item level'a göre score hesaplaması InGamePanel'de bulunan _totalScore'da tutulup UI'a yazıldı.
        - Burada user score data için ayrı bir sistem yazılmalı mı? 
            - Sorusuna case içerisinde belirtilmeyen bir katman getireceğini düşündüğümden UI'da tutulması yeterli olduğuna karar verildi.

    - Grid width ve height değerleri kod tarafından değiştirilebilir ve sistem bununla çalışır fakat case dosyasında 5x5 olması özellikle verildiği için camera için herhangi bir config yapılmadı.
        - Yani eğer Constants-> Grid -> width ve height değerlerini değiştirip test etmek isterseniz sistem çalışır ama sahneden camera size ve position ile ayarlamak gerekecektir

    - Diamond görselleri bilinçli 4 adet "Max Level" testi daha hızlı olması adına koyuldu.



