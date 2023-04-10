# Password Manager Webassembly
<h4> This is a UpSchool team sample project.</h4>
<p>In this project our goal is accomplish the manage of our personal infos like social media account passwords, credit card infos, personal notes, our addresses and generate strong passwords.</p>
<p>Also we are using  jasontaylordev's clean architecture repository to help us getting a simple and understandable software architecture to our own project.</p>

<h4> Here is a mockup of a project. it's gonna be similar to this:</h4>

![Untitled (1)](https://user-images.githubusercontent.com/30401423/230800537-66ad0a36-717c-4c4b-9572-5f9f65553be8.png)

#
<h4>According to create the project we are getting assigned tasks. At this stage my assignment is:</h4>
<p>1️⃣   I have to configure the fields in our "Address", "Note", "NoteCategory" entities correctly for the database.</p>

<p>2️⃣   I have to configure the relation between User and Address. One User can have many Addresses  (one-to-many) </p>

<p>3️⃣   I have to configure the relation between Note and Category. One Note can have many Category and One Category can have many Note. (many-to-many)</p>

<p>4️⃣   For "Address", Add, Update, Delete and HardDelete Commands must be present in the "CQRS" structure.</p>

<p>5️⃣   For "Address", GetById and GetAll Query must be created in "CQRS" structure.</p>

#
<h4>And this is what added to the project according to accomplish the tasks:</h4>
<p>:white_check_mark:  I configure the 
<ul>
<li>
<strong>
\UpSchool\UpStorage\src\Domain\Entities\Address.cs
</strong>
</li>
<li>
<strong>
\UpSchool\UpStorage\src\Domain\Entities\Note.cs
</strong>
</li>
<li>
<strong>
\UpSchool\UpStorage\src\Domain\Identity\User.cs
</strong>
</li>
<li>
<strong>
\UpSchool\UpStorage\src\Infrastructure\Persistence\Configurations\Application\AddressConfiguration.cs
</strong>
</li>
<li>
\UpSchool\UpStorage\src\Application\Common\Interfaces\IApplicationDbContext.cs
</strong>
</li>
<li>
<strong>
\UpSchool\UpStorage\src\Infrastructure\Persistence\Contexts\ApplicationDbContext.cs
</strong>
</li>

</ul>
folders and mark my code with the region label like this:
</p>


![odev](https://user-images.githubusercontent.com/30401423/230907774-4cfbe750-e57e-4522-a608-bbbcc7d4ae0a.PNG)
![odev2](https://user-images.githubusercontent.com/30401423/230907792-86253704-fb33-44f7-abbd-fddacb574bf0.PNG)
<p>This way the code that i wrote is much more detectable.</p>
<p>:white_check_mark:  Also i create the
<ul>
<li>
<strong>
\UpSchool\UpStorage\src\Domain\Entities\NoteCategory.cs
</strong>
</li>
<li>
<strong>
\UpSchool\UpStorage\src\Application\Features\Addresses\</strong> all files and folders about commands and queries within
</li>
<li>
<strong>
\UpSchool\UpStorage\src\WebApi\Controllers\AddressesController.cs
</strong>
</li>
</ul>
Folders and files.
</p>
<p>:recycle: I took some initiative and decided not to create GetById structor because we could give an id parameter to the GetAll function and get the wanted result.  </p> 
<p>:recycle: I decided to change the Address Type enums database variable to "smallint" from "int" that way database memory can be optimized. </p>

#
<h3>:tada: Here is my database diagram :tada:</h3> 

![databaseDiagram](https://user-images.githubusercontent.com/30401423/230912316-e50122d8-273c-4b5f-b06e-d29aac56e7fa.png)

#
<h3>My web Api look like this<h3>
![image](https://user-images.githubusercontent.com/30401423/230913692-6ba56554-9507-46bb-89cc-7c4ea54b1a27.png)

