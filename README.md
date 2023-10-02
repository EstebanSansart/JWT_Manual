# JWT Manual

Bienvenido al manual de JSON Web Token, donde se va a enseñar un paso a paso de como crear un JWT completamente funcional hecho de una forma no convencional; es decir, será hecho en Visual Studio Code y en un proyecto de 4 capas para mantener un mejor orden. Sin más que decir por ahora, aquí vamos.
## Índice

1. Creación del proyecto de 4 capas

1.1 Instalación del Batchfile

1.2 Ejecuta el archivo

1.3 Sigue los pasos del Batchfile

2. Configuraciones generales
   
2.1 Eliminar los "Nullable"

2.2 Agregar credenciales y llave

3. Entidades y configuraciones
   
3.1 Crear entidades

3.1.1 BaseEntityA

3.1.2 User

3.2 Crear contexto

3.3 Crear configuraciones

4. Primera migración
   
4.1 Configurar "Program.cs"

4.2 Realizar la migración

5. Crear clases de autorización
   
5.1 Clase "AuthRequest" (Solicitar Autorización)

5.2 Clase "AuthResponse" (Respuesta de la Autorización)

6. Crear los servicios
   
6.1 Crear "IAuthService" (Interfaz del servicio de autorización)

6.2 Crear "AuthService" (Clase del servicio de autorización)

6.2.1 GenerateToken()

6.2.2 ReturnToken()

7. Nuevas lineas a "Program.cs"

8. Crear y testear controllers (controladores)
   
8.1 CountryController

8.2 UserController

8.2.1 Iniciar servidor

8.2.2 Swagger

8.2.3 Uso de una Plataforma API

8.2.4 Obtener Token

9. Implementar RefreshToken
    
9.1 Nueva entidad y configuración

9.1.1 RefreshTokenRecord

9.1.2 Añadir al contexto

9.1.3 RefreshTokenRecordConfiguration

9.2 Modificar la clase "AuthResponse"

9.3 Nueva clase en "Custom"

9.4 Modificar la interfaz "IAuthService"

9.5 Modificar la clase "AuthService"

9.5.1 ReturnRefreshToken()

9.5.2 GenerateRefreshToken()

9.5.3 SaveRecordRefreshToken()

9.5.4 Modificar el método "ReturnToken"

9.5.5 Hacer la lógica de "ReturnRefreshToken"

9.6 Actualizar "UserController"

10. Testear el RefreshToken
    
10.1 Nueva petición

11. Guía de buenas practicas
## 1. Creación del proyecto de 4 capas

Para facilitar la creación de este proyecto, usaremos un "Batchfile" por autoría del programador: [trainingLeader](https://github.com/trainingLeader).

A continuación mostraré como manejar este archivo.
### 1.1 Instalación del Batchfile

Instala el archivo llamado "genproy.bat" como un ".zip" o ".rar",
o descarga directamente el archivo sin necesidad de comprimirlo,
y luego extráelo para obtener una carpeta con el respectivo
Batchfile. Adicionalmente, lleva el .bat a la carpeta donde
quieras realizar el proyecto.

### 1.2 Ejecuta el archivo

Al momento de ejecutar el Batchfile, es probable que te aparezca
la siguiente ventana:

![SCREENSHOT DE ADVERTENCIA](imgs/batchfileWarning.png)

En este caso, dale en "Ejecutar de todas formas", ya que es normal
que el sistema operativo lo reconozca como archivo desconocido y
con posibilidad de riesgo.

### 1.3 Sigue los pasos del Batchfile

A partir de aquí, no es necesario explicar más. Ya que es
completamente intuitivo el menú del archivo, y será rápido
tener todo ya instalado. Una vez ya todo esté hecho, puedes cerrar el Batchfile y luego abrir el proyecto en Visual Studio Code.
## 2. Configuraciones generales

Antes de comenzar a crear las tablas (entidades) y sus respectivas configuraciones, hay que realizar algunos ajustes generales para evitar molestias a futuro.
### 2.1 Eliminar los "Nullable"

Preferiblemente, para evitar "Warnings" más adelante, entra a los archivos de cada capa. En este caso serán:

- [nombreDeTuApi].csproj
- Application.csproj
- Domain.csproj
- Persistence.csproj

Y busca la siguiente línea de código:

![SCREENSHOT DE LINEA NULLABLE](imgs/nullableLine.png)

Borra esta línea de código en los 4 archivos ".csproj".
### 2.2 Agregar credenciales y llave

Entra a la siguiente ruta: 

    [[nombreDeTuApi]/appsettings.json]

Y define las credenciales debajo de la línea de código que dice "AllowedHosts". Un ejemplo de las credenciales pueden ser las siguientes:

    "ConnectionStrings": {
    "MainConnection":"server=localhost;user=root;password=123456;database=jwtManual"
    },

Recuerda que estas credenciales se pueden modificar en base a las tuyas.

Adicionalmente, define la llave debajo de las credenciales que acabas de añadir, hazlo de la siguiente manera:

    "JWTSettings":{
    "Key":"%LP!Y4bOeM@I(kqaRYF{-5nunH]ZAi6xF!JC:%T49{b?ghK''bgNi|%T`,w933Qa"
    }

Al igual que las credenciales, la "Key" puede ser como prefieras, aquí uso una con simbolos, numeros, mayúsculas y minúsculas para hacerla más compleja y dificil de adivinar.

El archivo en general debería verse de la siguiente forma:

![SCREENSHOT DE APP_SETTINGS_JSON](imgs/appSettingsJson.png)
## 3. Entidades y configuraciones

Por ahora solo se creará una sola entidad que servirá perfectamente para todo el proyecto de aquí en adelante, y así mismo, evitamos alargar el proyecto con más entidades.
### 3.1 Crear entidades

#### 3.1.1 BaseEntityA

Nos dirigimos a la siguiente ruta:

    [Domain/Entities]

Y creamos una carpeta llamada "Generics", donde crearemos una clase llamada "BaseEntityA.cs"; claro que, el nombre puede variar tanto como tu prefieras.

En esta entidad creamos solo una propiedad pública de tipo "int" que se va a llamar "Id".

Después de esto, la clase debería verse así:

![SCREENSHOT DE BASE_ENTITY_A](imgs/baseEntityA.png)

#### 3.1.2 User

Nos devolvemos a la ruta:

    [Domain/Entities]

Y ahí creamos una clase llamada "User.cs", hacemos que la clase herede de "BaseEntityA" de la siguiente forma:

    public class User : BaseEntityA

Y luego vamos a ponerle las siguientes propiedades:

- Pública y de tipo string: Username
- Pública y de tipo string: Password

### 3.2 Crear contexto

Nos dirigimos ahora a la carpeta (capa) "Persistence" y creamos una nueva clase llamada "[nombreDeTuApi]Context.cs".

Aquí, en las primeras líneas de código implementamos las siguientes rutas:

    using System.Reflection;
    using Domain.Entities;
    using Microsoft.EntityFrameworkCore;

Luego, hacemos que la clase herede de "DbContext" de esta manera:

    public class [nombreDeTuContexto] : DbContext


Heredamos de DbContext para aprovechar sus funcionalidades, como el seguimiento de cambios, las consultas, las migraciones, el soporte para múltiples bases de datos, el soporte para transacciones y el soporte para lazy loading.

Después, dentro de la clase añadimos la siguiente línea de código:

    public [nombreDeTuContexto](DbContextOptions<[nombreDeTuContexto]> conf) : base(conf){}

Después de esta línea de código, añadimos la entidad "User" con un "DbSet", quedando de esta forma:

    public DbSet<User> Users { get; set; }

Los DbSets son colecciones de entidades de un tipo determinado. Se utilizan para representar las tablas de la base de datos en el código.

Y al final de la clase (sin salirse de ella) agregamos las siguientes líneas:

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {}

    protected override void OnModelCreating(ModelBuilder modelBuilder){  
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }


Los métodos "OnConfiguring()" y "OnModelCreating()" permiten configurar el contexto de la base de datos y el modelo de la base de datos sin tener que escribir código explícito.

El método "ApplyConfigurationsFromAssembly()" permite aplicar todas las configuraciones de entidades que se encuentran en el ensamblado actual. Esto permite configurar el modelo de la base de datos sin tener que escribir código explícito en el método "OnModelCreating()".

### 3.3 Crear configuraciones

Ahora, nos dirigimos a la siguiente ruta:

    [Persistence/Data/Configuration]

Y creamos una nueva clase llamada "UserConfiguration.cs" en el cual se deben definir las propiedades, además de la llave principal y las relaciones que pueda tener. Antes de comenzar con esto, implementamos las siguientes rutas:

    using Domain.Entities;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

Además, hacemos que la clase herede de la siguiente manera:

    public class UserConfiguration : IEntityTypeConfiguration<User>

Aquí, se hereda de "IEntityTypeConfiguration<User>" para poder configurar la entidad User de forma fluida. Esto permite configurar la entidad sin tener que escribir código SQL complejo.

La interfaz "IEntityTypeConfiguration<User>" proporciona una serie de métodos que se pueden utilizar para configurar la entidad User. Por ejemplo, se puede utilizar para especificar el nombre de la tabla, la clave principal, las propiedades de la entidad, las restricciones de integridad y los datos predeterminados.

Al heredar de "IEntityTypeConfiguration<User>", la clase UserConfiguration puede utilizar estos métodos para configurar la entidad User. Esto simplifica el código de configuración de la entidad y permite a los desarrolladores centrarse en la lógica de negocio.

Luego, dentro de la clase, hacemos que el código quede de la siguiente forma:

![SCREENSHOT DE USER_CONFIGURATION](imgs/userConfiguration.png)

Aquí explicaré a modo de resumen esta parte del código:

- La tabla se llamará user.
- La clave principal será la propiedad Id.
- La propiedad Id será obligatoria y tendrá una estrategia de generación de valores de IdentityColumn.
- La propiedad Username será obligatoria y tendrá una longitud máxima de 20 caracteres.
- La propiedad Password será obligatoria y tendrá una longitud máxima de 20 caracteres.
- Se agregarán los datos predeterminados siguientes a la tabla user:

        {
        Id = 1,
        Username = "Admin",
        Password = "123",
        }


## 4. Primera migración

Una vez tenemos la entidad lista con su respectiva configuración y además ya añadida en el contexto, procedemos a realizar la primera migración que servirá para comprobar que la base de datos está bien estructurada y funciona con normalidad; pero primero necesitamos configurar unas cosas antes de proceder.
### 4.1 Configurar "Program.cs"

Nos dirigimos a la siguiente ruta:

    [[nombreDeTuApi]/Program.cs]

Y añadimos las siguientes rutas en las primeras líneas de código:

    using System.Reflection;
    using System.Text;
    using Microsoft.AspNetCore.Authentication.JwtBearer;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.IdentityModel.Tokens;
    using Persistence;

Ahora, teniendo en cuenta que previamente agregamos nuestras credenciales en el archivo "appsettings.json", agregamos las siguientes lineas de código:

    builder.Services.AddDbContext<[nombreDeTuApi]>(opts =>{
    string connection = builder.Configuration["ConnectionStrings:MainConnection"]?? throw new Exception("Error: Invalid Connection");
    opts.UseMySql(connection, ServerVersion.AutoDetect(connection));
    });

Estas lineas, agregan un servicio de contexto de base de datos a la colección de servicios de la aplicación web.

El contexto de base de datos es una clase que representa la base de datos. Proporciona una capa de abstracción para acceder a la base de datos, lo que facilita el desarrollo de aplicaciones.

El método AddDbContext() toma dos parámetros:

- El tipo de contexto de base de datos que se debe agregar. En este caso, el tipo de contexto de base de datos es ApiJWTManualContext.
- Una función que se utiliza para configurar el contexto de base de datos. En este caso, la función opts se utiliza para configurar el contexto de base de datos para que utilice MySQL como proveedor de la base de datos.

La función opts primero obtiene la cadena de conexión de la base de datos de la configuración de la aplicación web. Si la cadena de conexión de la base de datos no está configurada, se genera una excepción.

Una vez que la cadena de conexión de la base de datos se ha obtenido, la función opts utiliza el método UseMySql() para configurar el contexto de base de datos para que utilice MySQL como proveedor de la base de datos. El método UseMySql() toma dos parámetros:

- La cadena de conexión de la base de datos.
- La versión del servidor MySQL que se está utilizando. En este caso, el método ServerVersion.AutoDetect() se utiliza para detectar automáticamente la versión del servidor MySQL.

Después de que el contexto de base de datos ha sido configurado, la función opts devuelve el contexto de base de datos.

El método AddDbContext() agrega el contexto de base de datos a la colección de servicios de la aplicación web. La aplicación web puede utilizar el contexto de base de datos para acceder a la base de datos.
### 4.2 Realizar la migración

Primero, abrimos la terminal de Visual Studio Code y escribimos los siguientes comandos de formas separadas:

    dotnet build

Este es para compilar el proyecto y de paso comprobar si existen errores en este.

Luego escribimos:

    dotnet ef migrations add FirstMigration --project .\Persistence\ --startup-project .\[nombreDeTuApi]\ --output-dir ./Data/Migrations


Los parámetros del comando son:

- add: Este parámetro indica que se debe crear una nueva migración.
- FirstMigration: Este es el nombre de la migración.
- --project .\Persistence\: Este parámetro especifica el proyecto que contiene el contexto de la base de datos.
- --startup-project .\[nombreDeTuApi]\: Este parámetro especifica el proyecto de arranque de la aplicación.
- --output-dir ./Data/Migrations: Este parámetro especifica el directorio donde se deben guardar las migraciones.

Ahora escribimos el siguiente:

    dotnet ef database update --project .\Persistence\ --startup-project .\[nombreDeTuApi]\

Aquí los parámetros del comando son:

- update: Este parámetro indica que se deben aplicar las migraciones pendientes.
- --project .\Persistence\: Este parámetro especifica el proyecto que contiene el contexto de la base de datos.
- --startup-project .\ApiJWTManual\: Este parámetro especifica el proyecto de arranque de la aplicación.

Terminado esto, ya debería estar la base de datos funcionando correctamente y podemos comprobarlo en "MySQL Workbench" por decir un ejemplo.
## 5. Crear clases de autorización

Una vez ya funcionando la base de datos como debería, comenzamos a implementar las clases que harán funcional nuestro JSON Web Token.
### 5.1 Clase "AuthRequest" (Solicitar Autorización)

Nos dirigimos a la siguiente ruta:

    [[nombreDeTuApi]/Dtos]

Y creamos una nueva carpeta llamada "Custom", donde vamos a crear una nueva clase llamada "AuthRequest".

Aquí, agregamos dos propiedades exactamente iguales a las que habíamos hecho en la entidad "User" las cuales son:

- Username
- Password
### 5.2 Clase "AuthResponse" (Respuesta de la Autorización)

En la misma ruta de "AuthRequest", creamos una nueva clase llamada "AuthResponse" que tendrá las siguientes propiedades:

- Pública y de tipo string: Token
- Pública y de tipo booleano: Result
- Pública y de tipo string: Msg
## 6. Crear los servicios

Nos dirigimos a la siguiente ruta:

    [[nombreDeTuRuta]/Services]

Aquí vamos a definir los servicios de autorización para el token.
### 6.1 Crear "IAuthService" (Interfaz del servicio de autorización)

Comenzamos agregando una ruta de la siguiente forma:

    using ApiJWTManual.Dtos.Custom;

Luego, dentro de la clase, ponemos la siguiente línea de código:

    Task<AuthResponse> ReturnToken(AuthRequest auth);

Esto declara un método asíncrono llamado ReturnToken() que devuelve un valor de tipo AuthResponse de forma asíncrona.

Un método asíncrono es un método que puede devolver un resultado sin bloquear el hilo de ejecución actual. Esto es útil para operaciones que pueden tardar en completarse, como realizar una llamada a una base de datos o a un servicio web.
### 6.2 Crear "AuthService" (Clase del servicio de autorización)

Aquí comenzaremos añadiendo las siguientes rutas:

    using Microsoft.IdentityModel.Tokens;
    using System.IdentityModel.Tokens.Jwt;
    using System.Security.Claims;
    using System.Text;
    using Domain.Entities;
    using [nombreDeTuApi].Dtos;
    using [nombreDeTuApi].Dtos.Custom;
    using Microsoft.Extensions.Configuration;
    using Microsoft.AspNetCore.Mvc;
    using Persistence;
    using Microsoft.VisualBasic;
    using System.Security.Cryptography;

Ahora la clase va a heredar de la interfaz "IAuthService", generando un error; para solucionarlo se va a agregar la siguiente línea de código dentro de la clase:

    public async Task<AuthResponse> ReturnToken(AuthRequest auth)
    {
        throw new NotImplementedException();
    }

Esto es algo que se va a dejar momentáneamente ya que más tarde se va a modificar.

Consiguiente a esto, al principio dentro de la clase vamos a agregar estas lineas de código:

    private readonly [nombreDeTuContexto] _context;
    private readonly IConfiguration _configuration;

    public AuthService([nombreDeTuContexto] context, IConfiguration configuration)
    {
        _context = context;
        _configuration = configuration;
    }

El constructor de la clase AuthService toma dos parámetros:

- [nombreDeTuContexto]: Una instancia del contexto de la base de datos.
- IConfiguration: Una instancia de la configuración de la aplicación.

El constructor inyecta estas dependencias en la clase AuthService, lo que permite que la clase acceda a la base de datos y a la configuración de la aplicación.

Debajo del constructor comenzaremos a definir lo métodos que vamos a necesitar para los JSON Web Tokens.

#### 6.2.1 GenerateToken()

El primer método que se va a implementar va a ser el siguiente:

    private string GenerateToken(string IdUser){
        var key = _configuration.GetValue<string>("JWTSettings:Key");
        var keyBytes = Encoding.ASCII.GetBytes(key);
        var claims = new ClaimsIdentity();
        claims.AddClaim(new Claim(ClaimTypes.NameIdentifier, IdUser));
        var credentialToken = new SigningCredentials(
            new SymmetricSecurityKey(keyBytes),
            SecurityAlgorithms.HmacSha256Signature
        );
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = claims,
            Expires = DateTime.UtcNow.AddMinutes(1),
            SigningCredentials = credentialToken
        };
        var tokenHandler = new JwtSecurityTokenHandler();
        var tokenConfig = tokenHandler.CreateToken(tokenDescriptor);

        string tokenCreated = tokenHandler.WriteToken(tokenConfig);

        return tokenCreated;
    }

El método GenerateToken() genera un token JWT para el usuario dado. El método toma el identificador del usuario como parámetro y devuelve el token JWT como una cadena.

El método GenerateToken() funciona de la siguiente manera:

- Obtiene la clave secreta de la configuración de la aplicación.
- Convierte la clave secreta a una matriz de bytes.
- Crea un objeto ClaimsIdentity y agrega un reclamo al objeto con el identificador del usuario.
- Crea un objeto SigningCredentials con la clave secreta y el algoritmo de firma HMACSHA256.
- Crea un objeto SecurityTokenDescriptor con el sujeto, la fecha de expiración (1 minuto) y las credenciales de firma.
- Crea un objeto JwtSecurityTokenHandler y utiliza el objeto para crear un token JWT.
- Devuelve el token JWT como una cadena.

#### 6.2.2 ReturnToken()

Ahora si se va a modificar el método que se había agregado anteriormente llamado "ReturnToken". El método va a quedar de la siguiente forma:

    public async Task<AuthResponse> ReturnToken(AuthRequest auth)
    {
        var userFound = _context.Users.FirstOrDefault(x =>
            x.Username == auth.Username &&
            x.Password == auth.Password
        );
        if(userFound == null){
            return await Task.FromResult<AuthResponse>(null);
        }
        string tokenCreated = GenerateToken(userFound.Id.ToString());

        return new AuthResponse(){Token = tokenCreated, Result = true, Msg = "OK"};
    }

Primero, el código usa el método FirstOrDefault() para buscar un usuario en la tabla _context.Users de la base de datos cuyo nombre de usuario y contraseña coincidan con los proporcionados en el objeto AuthRequest. Si se encuentra un usuario, el código genera un token JWT usando el método GenerateToken(). El método GenerateToken() toma el ID del usuario como entrada y devuelve un token JWT que contiene el ID del usuario como una afirmación.

Finalmente, el código devuelve un objeto AuthResponse que contiene el token JWT y un mensaje de éxito.
## 7. Nuevas lineas a "Program.cs"

De aquí, pasamos al archivo "Program.cs" para agregar nuevas líneas de código. Estas nuevas líneas las vamos a añadir después de la línea que dice:

    builder.Services.AddSwaggerGen();

Y antes de la línea que dice:

    var app = builder.Build();

Las nuevas líneas de código son las siguientes:

    builder.Services.AddScoped<IAuthService, AuthService>();

Esta línea agrega el servicio AuthService al contenedor de dependencias de la aplicación. El servicio AuthService es el que se encargará de generar y validar los tokens JWT.

Luego agregamos las siguientes:

    var key = builder.Configuration.GetValue<string>("JWTSettings:Key");
    var keyBytes = Encoding.ASCII.GetBytes(key);

Estas líneas obtienen la clave secreta de la aplicación desde el archivo appsettings.json y la convierten a una matriz de bytes. La clave secreta se utiliza para firmar los tokens JWT.

Ahora se agregará el siguiente bloque:

    builder.Services.AddAuthentication(config =>{
        config.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        config.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    }).AddJwtBearer(config => {
        config.RequireHttpsMetadata = false;
        config.SaveToken = true;
        config.TokenValidationParameters = new TokenValidationParameters {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(keyBytes),
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero
        };
    });

Este bloque de código agrega la autenticación JWT a la aplicación. La primera línea establece que el esquema de autenticación predeterminado es JwtBearerDefaults.AuthenticationScheme. Esto significa que todos los controladores de la aplicación estarán protegidos por autenticación JWT.

La segunda línea agrega el middleware de autenticación JWT a la aplicación. El middleware de autenticación JWT es el que se encargará de validar los tokens JWT que se pasen en las solicitudes.

La tercera línea establece que no se requiere HTTPS para la autenticación JWT. Esto es útil para el desarrollo, pero se recomienda habilitar HTTPS para la producción.

La cuarta línea establece que los tokens JWT se deben guardar en la memoria caché del navegador del usuario. Esto puede mejorar el rendimiento de la aplicación, pero también puede aumentar el riesgo de ataques XSS.

La quinta línea establece los parámetros de validación de tokens JWT. En este caso, estamos validando la clave secreta, la vida útil del token y que el token no haya expirado.

Terminado esto, en las últimas lineas, agregamos una nueva línea de código la cual es la siguiente:

    app.UseAuthentication();

La línea app.UseAuthentication(); agrega el middleware de autenticación a la aplicación. El middleware de autenticación es el que se encarga de validar los tokens JWT que se pasen en las solicitudes.
## 8. Crear y testear controllers (controladores)

Nos dirigimos a la siguiente ruta:

    [[nombreDeTuApi]/Controllers]

Y aquí crearemos los controladores necesarios para poder hacer las peticiones.




### 8.1 CountryController

Ahora crearemos una nueva clase llamada "CountryController.cs". Una vez creado, dejamos este controlador con una base de la siguiente manera:

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;

    namespace ApiJWTManual.Controllers
    {
        [Route("api/[controller]")]
        [ApiController]

        public class CountryController : ControllerBase
        {
        
        }
    }

Una vez la base esté lista, comenzamos a agregar las siguientes líneas de código dentro de la clase:

    [Authorize]
    [HttpGet]
    [Route("Lista")]
    public async Task<IActionResult> Lista(){
        var countryList = await Task.FromResult(new List<string>{"Japan","Norway","Netherlands","Switzerland"});
        return Ok(countryList); 
    }

Después de esto, la clase en general debería verse así:

![SCREENSHOT DE COUNTRY_CONTROLLER](imgs/countryController.png)
### 8.2 UserController

Ahora vamos a proceder a crear el siguiente controlador llamado "UserController.cs".

Este controlador debería tener la misma base que el controlador anterior, y adicionalmente se le agregan las siguientes rutas:

    using ApiJWTManual.Dtos.Custom;
    using ApiJWTManual.Services;
    using System.IdentityModel.Tokens.Jwt;

Ahora, dentro de la clase se deben agregar las siguientes lineas de código:

    private readonly IAuthService _authService;

    public UserController(IAuthService authService)
    {
        _authService = authService;
    }
    [HttpPost]
    [Route("Autenticar")]
    public async Task<IActionResult> Autenticar([FromBody] AuthRequest auth){
        var AuthResult = await _authService.ReturnToken(auth);
        if(AuthResult == null)
            return Unauthorized();
        return Ok(AuthResult);
    }

Una vez hecho esto, ya podemos testear el controlador siguiendo estos pasos:

#### 8.2.1 Iniciar servidor

Abre una nueva terminal en Visual Studio Code, y escribe el siguiente comando:

    cd [nombreDeTuApi]

Acto seguido, se escribe el siguiente comando:

    dotnet run

Escribir este comando hace que se inicie un servidor donde se podrá ver la API que tenemos hasta ahora. Se debería ver de la siguiente manera:

![SCREENSHOT DE DOTNET_RUN](imgs/dotnetRun.png)

#### 8.2.2 Swagger

Una vez lograda esta respuesta de la terminal, copiamos la URL señalada en la anterior imagen, y la pegamos en un navegador junto a unas adiciones de "swagger", quedando de la siguiente manera:

    http://localhost:5092/swagger/index.html

Cabe aclarar que el puerto suele ser diferente, pero no afecta en nada lo que se quiere lograr.

Entramos a la URL que recién escribimos y deberiamos estar en la siguiente página:

![SCREENSHOT DE SWAGGER](imgs/swagger.png)

#### 8.2.3 Uso de una Plataforma API

Ahora, abrimos la plataforma API para desarrolladores que prefieras. Algunos ejemplos son:

- Postman
- Thunder Client
- Insomnia

En este caso, se usará Thunder Client.

Una vez en Thunder Client, abrimos una nueva petición de tipo GET y ponemos la siguiente URL:

    http://localhost:5092/api/Country/Lista

Ahora, enviamos la URL y esperamos la respuesta del servidor, que en este caso debería devolver un "401 Unauthorized". Esto es con la finalidad de comprobar que todavía no podemos acceder a la lista de paises porque no tenemos un Token que nos autorice para obtener estos datos.

#### 8.2.4 Obtener Token

Ahora, para tener autorización, creamos una nueva petición de tipo POST en Thunder Client que tendrá la siguiente URL:

    http://localhost:5092/api/User/Autenticar

Acto seguido, volvemos a la página de Swagger, entramos a la sección de "User" y comprobamos que es lo que pide esa petición. Una vez comprobado, podemos ver que pide las siguientes propiedades:

    {
        "username": "string",
        "password": "string"
    }

Copiamos estas propiedades y nos dirigimos a la nueva petición que hicimos en Thunder Client. Entramos a la división de "Body" y luego a la subdivisión "JSON", pegamos las propiedades y les agregamos la información necesaria. En este caso, vamos a poner los datos predeterminados que anteriormente habiamos definido en "UserConfiguration" quedando de la siguiente manera:

    {
        "username": "Admin",
        "password": "123"
    }

Una vez hecho esto, enviamos la petición y esperamos la respuesta del servidor, la cual como ejemplo debería ser la siguiente:

    {
        "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJuYW1laWQiOiIxIiwibmJmIjoxNjk2MjE4NzI4LCJleHAiOjE2OTYyMTg3ODgsImlhdCI6MTY5NjIxODcyOH0.yr81OTk7px_2GVcJeqgyElTuU_EYBqpKnV4EkFFDiWM",
        "result": true,
        "msg": "OK"
    }

Aclaro que el Token que genere cada vez que se haga la petición siempre va a ser diferente.

Ahora que tenemos el Token, lo copiamos y nos dirigimos a la primera petición GET de la lista de paises que queriamos generar. Vamos a la división "Auth" y luego a la subdivisión "Bearer". En la caja "Bearer Token" pegamos el Token que previamente copiamos. Acto seguido enviamos la petición y obtenemos la siguiente respuesta:

    [
        "Japan",
        "Norway",
        "Netherlands",
        "Switzerland"
    ]

Con esto comprobamos que el Token de autorización nos sirve para obtener la lista de paises sin ningún problema.

Aquí mismo, podemos comprobar que el tiempo de expiración del Token también se toma en cuenta. Recordemos que anteriormente, en el punto 6.2.1 al momento de implementar el método de generar el token, habiamos configurado que su tiempo de expiración durase 1 minuto. Así que, para comprobar que es verdad. Una vez obtenidos los datos de la lista de paises, esperamos 1 minuto o más, intentamos hacer la petición de nuevo, y volveremos a estar sin autorización.
## 9. Implementar RefreshToken

El punto de un refresh token es permitir que un usuario actualice su token de acceso sin tener que volver a iniciar sesión.

Los tokens de acceso tienen una vida útil limitada. Una vez que el token de acceso expira, el usuario debe iniciar sesión nuevamente para obtener un nuevo token. Esto puede ser un inconveniente para los usuarios, especialmente si tienen que iniciar sesión con frecuencia.

Los refresh tokens pueden ayudar a resolver este problema. Cuando un usuario inicia sesión, el servidor genera un token de acceso y un refresh token. El token de acceso se utiliza para autenticar al usuario en las solicitudes posteriores. El refresh token se almacena en el navegador del usuario y se puede usar para obtener un nuevo token de acceso cuando el token de acceso actual expira.
### 9.1 Nueva entidad y configuración

Para que podamos implementar el RefreshToken, se necesita una nueva tabla que tenga relación con "User". A continuación veremos como funciona.

#### 9.1.1 RefreshTokenRecord

Ahora se debe crear una nueva entidad que se llamará "RefreshTokenRecord.cs" que debería verse así:

![SCREENSHOT DE REFRESH_TOKEN_RECORD](imgs/refreshTokenRecord.png)

Además debemos agregar algo adicional a la entidad "User" que ya existe, y es la siguiente línea de código dentro de la clase:

    public virtual ICollection<RefreshTokenRecord> RefreshTokenRecords { get; } = new List<RefreshTokenRecord>();

De esta forma, la entidad "User" debería quedar de esta manera:

![SCREENSHOT DE USER](imgs/user.png)

#### 9.1.2 Añadir al contexto

Al igual que con "User", también debemos añadir esta nueva entidad en el contexto de nuestra API. Quedando de la siguiente manera:

![SCREENSHOT DE CONTEXT](imgs/context.png)

#### 9.1.3 RefreshTokenRecordConfiguration

Ahora podremos crear la configuración de la nueva entidad, y debe quedar de la siguiente forma:

![SCREENSHOT DE REFRESH_TOKEN_RECORD_CONFIGURATION](imgs/refreshTokenRecordConfiguration.png)

Y ahora resumiré esta configuración:

- Id: La clave principal de la entidad. Es un campo de tipo entero que se genera automáticamente.
- Token: El token de refresco. Es un campo de tipo cadena de texto con una longitud máxima de 500 caracteres.
- RefreshToken: El token de actualización. Es un campo de tipo cadena de texto con una longitud máxima de 200 caracteres.
- CreationDate: La fecha de creación del token de refresco. Es un campo de tipo fecha y hora.
- ExpirationDate: La fecha de expiración del token de refresco. Es un campo de tipo fecha y hora.
- IsActive: Un indicador que indica si el token de refresco está activo. Es un campo de tipo booleano.

La entidad RefreshTokenRecord también tiene una relación con la entidad User. Esta relación es de uno a muchos, lo que significa que un usuario puede tener muchos tokens de refresco, pero un token de refresco solo puede pertenecer a un usuario.
### 9.2 Modificar la clase "AuthResponse"

Ahora que estamos implementando el RefreshToken, serán necesarias algunas modificaciones en la clase que habíamos creado previamente.

Aquí solo debemos añadir una nueva propiedad pública de tipo String llamada "RefreshToken", quedando de la siguiente forma:

![SCREENSHOT DE AUTH_RESPONSE](imgs/authResponse.png)
### 9.3 Nueva clase en "Custom"

Esta nueva clase recibirá el nombre de "RefreshTokenRequest" y tendrá dos propiedades, quedando de la siguiente forma:

![SCREENSHOT DE REFRESH_TOKEN_REQUEST](imgs/refreshTokenRequest.png)
### 9.4 Modificar la interfaz "IAuthService"

Donde vamos a añadir una nueva propiedad para devolver el RefreshToken y que quede de la siguiente manera:

![SCREENSHOT DE IAUTH_SERVICE](imgs/iAuthService.png)
### 9.5 Modificar la clase "AuthService"

Ahora, se deben implementar nuevos métodos para lograr generar y devolver un RefreshToken. Haremos esto con los siguientes pasos:

#### 9.5.1 ReturnRefreshToken()

Al entrar a la clase "AuthService" (Después de haber implementado la nueva propiedad en su interfaz), tendremos un error la cual podremos solucionar añadiendo un nuevo método llamado "ReturnRefreshToken" de la siguiente manera:

    public async Task<AuthResponse> ReturnRefreshToken(RefreshTokenRequest refreshTokenRequest, int UserId)
    {
        throw new NotImplementedException();
    }

Por ahora se quedará así, ya que luego lo vamos a modificar.

#### 9.5.2 GenerateRefreshToken()

Debajo del método ya existente "GenerateToken()" se va a crear uno nuevo llamado "GenerateRefreshToken" que contendrá lo siguiente:

    private string GenerateRefreshToken(){
        var byteArray = new byte[64];
        var refreshToken = "";

        using (var rng = RandomNumberGenerator.Create()){
            rng.GetBytes(byteArray);
            refreshToken = Convert.ToBase64String(byteArray);
        }
        return refreshToken;
    }

La clase RandomNumberGenerator proporciona una forma de generar números aleatorios seguros criptográficamente. Esto es importante porque el token de actualización debe ser impredecible y difícil de falsificar.

El método Convert.ToBase64String() convierte el array de bytes en una cadena Base64. La codificación Base64 es una forma de representar datos binarios en un formato de texto. Esto es útil porque el token de actualización se almacenará y transmitirá como texto.

Una vez que se genera el token de actualización, se puede almacenar en la aplicación cliente y se puede utilizar para obtener nuevos tokens de acceso cuando sea necesario.

#### 9.5.3 SaveRecordRefreshToken()

Debajo del método "GenerateRefreshToken" crearemos un nuevo método llamado "SaveRecordRefreshToken" que se verá de la siguiente manera:

    private async Task<AuthResponse> SaveRecordRefreshToken(
        int userId,
        string token,
        string refreshToken
    ){
        var recordRefreshToken = new RefreshTokenRecord
        {
            UserId = userId,
            Token = token,
            RefreshToken = refreshToken,
            CreationDate = DateTime.UtcNow,
            ExpirationDate = DateTime.UtcNow.AddMinutes(2)
        };
        await _context.RefreshTokenRecords.AddAsync(recordRefreshToken);
        await _context.SaveChangesAsync();
        return new AuthResponse{Token = token, RefreshToken = refreshToken, Result = true, Msg = "OK"};
    }

El método SaveRecordRefreshToken() guarda un registro de token de actualización en la base de datos. Este registro contiene la siguiente información:

- UserId: el identificador del usuario
- Token: el token de acceso
- RefreshToken: el token de actualización
- CreationDate: la fecha de creación del registro
- ExpirationDate: la fecha de caducidad del registro (2 minutos)

El método funciona de la siguiente manera:

- Crea una nueva instancia de la clase RefreshTokenRecord y la inicializa con los valores proporcionados.
- Agrega el registro a la base de datos utilizando el método AddAsync().
- Guarda los cambios en la base de datos utilizando el método SaveChangesAsync().
- Devuelve una instancia de la clase AuthResponse que contiene el token de acceso y el token de actualización.

El método SaveRecordRefreshToken() es importante porque permite que la aplicación cliente almacene el token de actualización y lo utilice para obtener nuevos tokens de acceso cuando sea necesario.

#### 9.5.4 Modificar el método "ReturnToken"

A continuación se modificará el método "ReturnToken" ya que ahora tenemos uno que genera RefreshToken. Teniendo en cuenta esto, el método "ReturnToken" tendrá dos nuevas lineas de código:

- Una nueva variable de tipo String llamada "tokenCreated"
- Un nuevo return que ahora devuelve el método SaveRecordRefreshToken con nuevos parámetros.

El método "ReturnToken" debería quedar de esta manera:

    public async Task<AuthResponse> ReturnToken(AuthRequest auth)
    {
        var userFound = _context.Users.FirstOrDefault(x =>
            x.Username == auth.Username &&
            x.Password == auth.Password
        );
        if(userFound == null){
            return await Task.FromResult<AuthResponse>(null);
        }
        string tokenCreated = GenerateToken(userFound.Id.ToString());

        string refreshTokenCreated = GenerateRefreshToken();

        //return new AuthResponse(){Token = tokenCreated, Result = true, Msg = "OK"};
        return await SaveRecordRefreshToken(userFound.Id, tokenCreated, refreshTokenCreated);
    }

#### 9.5.5 Hacer la lógica de "ReturnRefreshToken"

Ahora volviendo al método que hicimos antes llamado "ReturnRefreshToken", quitaremos la línea de código que dice:

    throw new NotImplementedException();

Y en su lugar, haremos la lógica para que quede de la siguiente forma:

    public async Task<AuthResponse> ReturnRefreshToken(RefreshTokenRequest refreshTokenRequest, int userId)
    {
        var refreshTokenFound = _context.RefreshTokenRecords.FirstOrDefault(x =>
            x.Token == refreshTokenRequest.ExpiredToken &&
            x.RefreshToken == refreshTokenRequest.RefreshToken &&
            x.UserId == userId
        );

        if(refreshTokenFound == null)
            return new AuthResponse {Result = false, Msg = "The Refresh Token doesn´t exist"};
            
        var refreshTokenCreated = GenerateRefreshToken();
        var tokenCreated = GenerateToken(userId.ToString());

        return await SaveRecordRefreshToken(userId, tokenCreated, refreshTokenCreated);
        }

El método ReturnRefreshToken() se utiliza para obtener un nuevo token de acceso utilizando un token de actualización existente. El método funciona de la siguiente manera:

- Busca el registro de token de actualización en la base de datos que coincide con el token de actualización proporcionado.
- Si el registro de token de actualización no existe, devuelve una respuesta de error.
- Genera un nuevo token de acceso y un nuevo token de actualización.
- Guarda el nuevo registro de token de actualización en la base de datos.
- Devuelve una respuesta que contiene el nuevo token de acceso y el nuevo token de actualización.

Finalizado esto. La clase "AuthService" debería verse así:

![SCREENSHOT DE AUTH_SERVICE](imgs/authService.png)
### 9.6 Actualizar "UserController"

A continuación vamos a añadir un nuevo método a "UserController" el cual se va a llamar "ObtenerRefreshToken" y se verá así:

    [HttpPost]
    [Route("ObtenerRefreshToken")]
    public async Task<IActionResult> ObtenerRefreshToken([FromBody] RefreshTokenRequest request){
        var tokenHandler = new JwtSecurityTokenHandler();
        var supposedlyExpiredToken = tokenHandler.ReadJwtToken(request.ExpiredToken);

        if(supposedlyExpiredToken.ValidTo > DateTime.UtcNow)
            return BadRequest(new AuthResponse{Result = false, Msg = "The token has not expired"});

        string userId = supposedlyExpiredToken.Claims.First(x =>
            x.Type == JwtRegisteredClaimNames.NameId).Value.ToString();

        var authResponse = await _authService.ReturnRefreshToken(request, int.Parse(userId));

        if(authResponse.Result)
            return Ok(authResponse);
        else
            return BadRequest(authResponse);
    }

El método ObtenerRefreshToken() se utiliza para obtener un nuevo token de acceso utilizando un token de actualización existente. El método funciona de la siguiente manera:

- Lee el token de acceso caducado utilizando el manejador de tokens JWT.
- Verifica que el token de acceso caducado sea válido y que aún no haya expirado.
- Obtiene el identificador de usuario del token de acceso caducado.
- Llama al método ReturnRefreshToken() del servicio de autenticación para obtener un nuevo token de acceso y un nuevo token de actualización.
- Si la solicitud de token de actualización es exitosa, devuelve una respuesta que contiene el nuevo token de acceso y el nuevo token de actualización.
- Si la solicitud de token de actualización falla, devuelve una respuesta de error.

Una vez hecho esto, el controlador "UserController" debería verse de la siguiente manera:

![SCREENSHOT DE USER_CONTROLLER](imgs/userController.png)
## 10. Testear el RefreshToken

Ahora abriremos la terminal de Visual Studio Code, y volveremos a correr el servidor como lo hicimos anteriormente por lo que me saltaré los comandos para hacerlo. A continuación mostraré los pasos para poner a prueba el RefreshToken.
### 10.1 Nueva petición

Después de poner a funcionar el servidor, nos dirigimos a Thunder Client para crear una nueva petición de tipo POST que tendrá la siguiente URL:

    http://localhost:5092/api/User/ObtenerRefreshToken

Ahora en la división "Body" y subdivisión "JSON" pondremos lo que pide esta petición en específico. Recuerdemos que esto lo podemos saber entrando a Swagger y entramos a la sección de "User" pero miramos la segunda petición tipo POST que nos muestra. En este caso, pide las siguientes propiedades:

    {
        "expiredToken": "string",
        "refreshToken": "string"
    }

Una vez esto esté en el Body/JSON de la petición, vamos a la petición para autenticar nuestro usuario. Esta petición la probamos en el punto 8.2.4 para poderlo recordar.

Una vez hecha esta petición, veremos que el servidor nos devolverá una respuesta como esta por dar un ejemplo:

    {
        "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJuYW1laWQiOiIxIiwibmJmIjoxNjk2MjI1OTc1LCJleHAiOjE2OTYyMjYwMzUsImlhdCI6MTY5NjIyNTk3NX0.wpyhrEVHzNMQOiGeYB2SDYfTPpNzLrXOhEY35Nf6WY4",
        "refreshToken": "QNXVQY8veXBy8V1FZ0r0UckDyAA2Ocjvy+AzOLlkgb6EgFROVnTMB2lp2lPoz171AbWUWYrssFoHeBiZ99EU+g==",
        "result": true,
        "msg": "OK"
    }

Vuelvo a recordar que el Token y el RefreshToken siempre son diferentes cada vez que se realiza esta petición.

Una vez hecha esta petición, copiamos el Token que ha generado y lo pegamos en el Auth/Bearer de la petición para listar los paises. Enviamos la petición y nos devolverá los paises correctamente.

Ahora, esperamos 1 minuto para que el token pueda vencer, pero ahora vamos a hacer algo diferente:

- Volveremos a la petición que genera el Token y el RefreshToken
- Copiamos el Token normal
- Vamos a la nueva petición localhost:5092/api/User/ObtenerRefreshToken
- Vamos a la subdivisión Body/JSON
- Cambiamos los valores de la propiedad "expiredToken" con el Token normal que acabamos de copiar
- Volvemos a la petición que genera el Token y el RefreshToken de nuevo
- Copiamos el RefreshToken
- Vamos a la petición ObtenerRefreshToken y cambiamos los valores de la propiedad "refreshToken" con el RefreshToken que acabamos de copiar
- Enviamos la petición

El servidor debería devolver una respuesta como la siguiente por dar un ejemplo:

    {
        "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJuYW1laWQiOiIxIiwibmJmIjoxNjk2MjI2NTI3LCJleHAiOjE2OTYyMjY1ODcsImlhdCI6MTY5NjIyNjUyN30.VN_wh80kejEAitOLxCaEfbPL8HH0-1OyOzCxsiBYqWk",
        "refreshToken": "3tXIJ/rwPQc5Oiz0sNKW5Wnmt+FyUEUbaj+vL0sHMazij6o54giyOsqJv57u/AabmKhGx0dbd1mZFZoiukOjGg==",
        "result": true,
        "msg": "OK"
    }

Ahora, una vez obtenida esta respuesta, copiamos el Token normal que nos ha dejado. Vamos a la petición para listar paises y pegamos el nuevo Token normal que hemos copiado en la subdivisión Auth/Bearer. Acto seguido, enviamos la petición y veremos que el servidor nos devuelve la lista de paises sin ningún problema.

Ahora para comprobar que el RefreshToken funciona bien, recordemos que antes en el punto 9.6.3 le pusimos un tiempo de expiración de 2 minutos. Pues esperamos este tiempo y al volver a realizar la petición, ya no estaremos autorizados.

Luego de comprobar esto, aumentamos el tiempo de expiración del RefreshToken las horas que consideremos adecuadas y terminaremos.
## 11. Guía de buenas practicas

Ya finalizado el proyecto, hablemos un poco de las buenas practicas que se pueden aplicar en este trabajo o similares.

- Todos los nombres en inglés: Esta es una de las mejores practicas que se pueden hacer, ya que se maneja el idioma universal y puede ser entendible para todos los que manejen las bases de datos.
- Orden de los nombres: En este caso, por ejemplo, al momento de definir un Id se hace lo siguiente: "UserId" pero al momento de definir otro se le nombra como "Id_Provider". La idea es que todos comiencen con la palabra "Id" y luego se haga la adición del nombre específico de este Id o viceversa, pero que sea un orden para todos.
- Identar donde se debe identar: Aquí quiero dar a entender que hay veces en las que se debe identar para mantener una vista más ordenada del código. Por ejemplo:

        "objeto1" = {
            "nombre1" = {
                "nombre2" : "valor2",
                "nombre3" : "valor3"
            }
        },
        "objeto2" = {
            "nombre4" = {
                "nombre5" : "valor5",
                "nombre6" : "valor6",
                "nombre7" : "valor7
            },
            "nombre8" = {}
        }

## Author

- [@EstebanSansart](https://github.com/EstebanSansart)
