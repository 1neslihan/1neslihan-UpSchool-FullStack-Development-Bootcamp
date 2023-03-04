<h2> Memento Pattern Örneği </h2>
<ul> 
<li> 
Bu çalışmada <strong>\UpSchool\UpSchool.Domain\Utilities\PasswordGenerator.cs </strong> isimli şifrelerimizi üreten fonksiyonu barındıran dosyaya memento pattern
ekledik.
</li>
<li>
<strong>PasswordGenerator.cs </strong> dosyamızın bulunduğu dizine <strong> PasswordGeneratorMemento.cs </strong> ve <strong> PasswordGeneratorCareTaker.cs </strong>
classlarını ekledik. Bu classlar yardımıyla <strong> PasswordGeneratorPage.razor </strong> içerisinde PasswordGenerator'dan üretilen şifrelerin geçmiş durumlarını depolayıp 
daha sonra butonlar yardımıyla bunları ekrana taşıdık.
</li>
</ul>


![MementoPattern](https://user-images.githubusercontent.com/30401423/222893807-04479864-4488-4d08-a4d5-6df0fd6b5f3b.gif)
