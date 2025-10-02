using BookCatalog.Domain.Entities;

namespace BookCatalog.Infrastructure.Data.Extentions;

public static class InitialData
{
    public static readonly List<Author> Authors = new()
    {
        new Author
        {
            Id = Guid.Parse("550e8400-e29b-41d4-a716-446655440001"),
            FirstName = "Gabriel",
            LastName = "García Márquez",
            DateOfBirth = new DateTime(1927, 3, 6, 0, 0, 0, DateTimeKind.Utc),
            DateOfDeath = new DateTime(2014, 4, 17, 0, 0, 0, DateTimeKind.Utc),
            Biography = "Escritor colombiano, considerado um dos autores mais significativos do século XX. Ganhou o Prêmio Nobel de Literatura em 1982 e é conhecido principalmente por suas obras que exemplificam o realismo mágico."
        },
        new Author
        {
            Id = Guid.Parse("550e8400-e29b-41d4-a716-446655440002"),
            FirstName = "J.K.",
            LastName = "Rowling",
            DateOfBirth = new DateTime(1965, 7, 31, 0, 0, 0, DateTimeKind.Utc),
            Biography = "Escritora britânica, mundialmente conhecida por ser a autora da série Harry Potter. Suas obras venderam mais de 500 milhões de cópias em todo o mundo."
        },
        new Author
        {
            Id = Guid.Parse("550e8400-e29b-41d4-a716-446655440003"),
            FirstName = "George",
            LastName = "Orwell",
            DateOfBirth = new DateTime(1903, 6, 25, 0, 0, 0, DateTimeKind.Utc),
            DateOfDeath = new DateTime(1950, 1, 21, 0, 0, 0, DateTimeKind.Utc),
            Biography = "Escritor e jornalista inglês, conhecido por suas obras distópicas '1984' e 'A Revolução dos Bichos'. Suas obras abordam temas de totalitarismo, linguagem e política."
        },
        new Author
        {
            Id = Guid.Parse("550e8400-e29b-41d4-a716-446655440004"),
            FirstName = "Agatha",
            LastName = "Christie",
            DateOfBirth = new DateTime(1890, 9, 15, 0, 0, 0, DateTimeKind.Utc),
            DateOfDeath = new DateTime(1976, 1, 12, 0, 0, 0, DateTimeKind.Utc),
            Biography = "Escritora britânica de romances policiais, criadora dos detetives Hercule Poirot e Miss Marple. É uma das escritoras mais traduzidas do mundo."
        },
        new Author
        {
            Id = Guid.Parse("550e8400-e29b-41d4-a716-446655440005"),
            FirstName = "Machado",
            LastName = "de Assis",
            DateOfBirth = new DateTime(1839, 6, 21, 0, 0, 0, DateTimeKind.Utc),
            DateOfDeath = new DateTime(1908, 9, 29, 0, 0, 0, DateTimeKind.Utc),
            Biography = "Escritor brasileiro, considerado um dos maiores nomes da literatura brasileira e mundial. Fundador da Academia Brasileira de Letras."
        },
        new Author
        {
            Id = Guid.Parse("550e8400-e29b-41d4-a716-446655440006"),
            FirstName = "Stephen",
            LastName = "King",
            DateOfBirth = new DateTime(1947, 9, 21, 0, 0, 0, DateTimeKind.Utc),
            Biography = "Escritor americano de ficção de terror, ficção sobrenatural, suspense, ficção científica e fantasia. Publicou mais de 60 romances e 200 contos."
        },
        new Author
        {
            Id = Guid.Parse("550e8400-e29b-41d4-a716-446655440007"),
            FirstName = "Jane",
            LastName = "Austen",
            DateOfBirth = new DateTime(1775, 12, 16, 0, 0, 0, DateTimeKind.Utc),
            DateOfDeath = new DateTime(1817, 7, 18, 0, 0, 0, DateTimeKind.Utc),
            Biography = "Romancista inglesa conhecida principalmente por seus seis grandes romances, que criticam a sociedade britânica do século XVIII."
        },
        new Author
        {
            Id = Guid.Parse("550e8400-e29b-41d4-a716-446655440008"),
            FirstName = "Paulo",
            LastName = "Coelho",
            DateOfBirth = new DateTime(1947, 8, 24, 0, 0, 0, DateTimeKind.Utc),
            Biography = "Escritor brasileiro, um dos autores mais lidos do mundo. Seu livro 'O Alquimista' foi traduzido para mais de 80 idiomas."
        }
    };

    public static readonly List<Genre> Genres = new()
    {
        new Genre
        {
            Id = Guid.Parse("660e8400-e29b-41d4-a716-446655440001"),
            Name = "Realismo Mágico",
            Description = "Gênero literário que combina elementos realistas com elementos fantásticos, apresentando o extraordinário como parte natural da realidade."
        },
        new Genre
        {
            Id = Guid.Parse("660e8400-e29b-41d4-a716-446655440002"),
            Name = "Fantasia",
            Description = "Gênero que apresenta elementos sobrenaturais, mágicos ou impossíveis, geralmente ambientado em mundos fictícios."
        },
        new Genre
        {
            Id = Guid.Parse("660e8400-e29b-41d4-a716-446655440003"),
            Name = "Ficção Distópica",
            Description = "Subgênero da ficção científica que retrata sociedades futuras opressivas ou indesejáveis."
        },
        new Genre
        {
            Id = Guid.Parse("660e8400-e29b-41d4-a716-446655440004"),
            Name = "Romance Policial",
            Description = "Gênero literário que se concentra na investigação de crimes, geralmente assassinatos, por um detetive profissional ou amador."
        },
        new Genre
        {
            Id = Guid.Parse("660e8400-e29b-41d4-a716-446655440005"),
            Name = "Realismo",
            Description = "Movimento literário que busca retratar a realidade de forma objetiva, sem idealizações."
        },
        new Genre
        {
            Id = Guid.Parse("660e8400-e29b-41d4-a716-446655440006"),
            Name = "Terror",
            Description = "Gênero literário que visa provocar sentimentos de medo, suspense e tensão no leitor."
        },
        new Genre
        {
            Id = Guid.Parse("660e8400-e29b-41d4-a716-446655440007"),
            Name = "Romance",
            Description = "Gênero literário que foca em relacionamentos amorosos e nas emoções dos personagens."
        },
        new Genre
        {
            Id = Guid.Parse("660e8400-e29b-41d4-a716-446655440008"),
            Name = "Ficção Filosófica",
            Description = "Gênero que explora questões filosóficas profundas através de narrativas ficcionais."
        },
        new Genre
        {
            Id = Guid.Parse("660e8400-e29b-41d4-a716-446655440009"),
            Name = "Autobiografia",
            Description = "Narrativa da vida de uma pessoa escrita por ela mesma."
        }
    };

    public static readonly List<Book> Books = new()
    {
        new Book
        {
            Id = Guid.Parse("770e8400-e29b-41d4-a716-446655440001"),
            Title = "Cem Anos de Solidão",
            Description = "A obra-prima de García Márquez narra a história da família Buendía ao longo de várias gerações na cidade fictícia de Macondo.",
            PublishedDate = new DateTime(1967, 6, 5, 0, 0, 0, DateTimeKind.Utc),
            ISBN = "978-85-359-0277-5",
            PageCount = 432,
            Publisher = "Editorial Sudamericana",
            AuthorId = Guid.Parse("550e8400-e29b-41d4-a716-446655440001"),
            GenreId = Guid.Parse("660e8400-e29b-41d4-a716-446655440001")
        },
        new Book
        {
            Id = Guid.Parse("770e8400-e29b-41d4-a716-446655440002"),
            Title = "O Amor nos Tempos do Cólera",
            Description = "Romance que narra a história de amor entre Florentino Ariza e Fermina Daza ao longo de mais de cinquenta anos.",
            PublishedDate = new DateTime(1985, 3, 6, 0, 0, 0, DateTimeKind.Utc),
            ISBN = "978-85-359-0278-2",
            PageCount = 368,
            Publisher = "Editorial Sudamericana",
            AuthorId = Guid.Parse("550e8400-e29b-41d4-a716-446655440001"),
            GenreId = Guid.Parse("660e8400-e29b-41d4-a716-446655440007")
        },
        new Book
        {
            Id = Guid.Parse("770e8400-e29b-41d4-a716-446655440003"),
            Title = "Harry Potter e a Pedra Filosofal",
            Description = "O primeiro livro da série Harry Potter, que narra as aventuras do jovem bruxo em sua descoberta do mundo mágico.",
            PublishedDate = new DateTime(1997, 6, 26, 0, 0, 0, DateTimeKind.Utc),
            ISBN = "978-85-325-1101-4",
            PageCount = 264,
            Publisher = "Bloomsbury",
            AuthorId = Guid.Parse("550e8400-e29b-41d4-a716-446655440002"),
            GenreId = Guid.Parse("660e8400-e29b-41d4-a716-446655440002")
        },
        new Book
        {
            Id = Guid.Parse("770e8400-e29b-41d4-a716-446655440004"),
            Title = "Harry Potter e a Câmara Secreta",
            Description = "O segundo livro da série, onde Harry retorna à Hogwarts e enfrenta novos mistérios e perigos.",
            PublishedDate = new DateTime(1998, 7, 2, 0, 0, 0, DateTimeKind.Utc),
            ISBN = "978-85-325-1102-1",
            PageCount = 288,
            Publisher = "Bloomsbury",
            AuthorId = Guid.Parse("550e8400-e29b-41d4-a716-446655440002"),
            GenreId = Guid.Parse("660e8400-e29b-41d4-a716-446655440002")
        },
        new Book
        {
            Id = Guid.Parse("770e8400-e29b-41d4-a716-446655440005"),
            Title = "1984",
            Description = "Romance distópico que retrata uma sociedade totalitária sob vigilância constante do 'Grande Irmão'.",
            PublishedDate = new DateTime(1949, 6, 8, 0, 0, 0, DateTimeKind.Utc),
            ISBN = "978-85-359-0123-5",
            PageCount = 416,
            Publisher = "Secker & Warburg",
            AuthorId = Guid.Parse("550e8400-e29b-41d4-a716-446655440003"),
            GenreId = Guid.Parse("660e8400-e29b-41d4-a716-446655440003")
        },
        new Book
        {
            Id = Guid.Parse("770e8400-e29b-41d4-a716-446655440006"),
            Title = "A Revolução dos Bichos",
            Description = "Fábula política que narra a revolta dos animais de uma fazenda contra seus donos humanos.",
            PublishedDate = new DateTime(1945, 8, 17, 0, 0, 0, DateTimeKind.Utc),
            ISBN = "978-85-359-0124-2",
            PageCount = 152,
            Publisher = "Secker & Warburg",
            AuthorId = Guid.Parse("550e8400-e29b-41d4-a716-446655440003"),
            GenreId = Guid.Parse("660e8400-e29b-41d4-a716-446655440003")
        },
        new Book
        {
            Id = Guid.Parse("770e8400-e29b-41d4-a716-446655440007"),
            Title = "Assassinato no Expresso do Oriente",
            Description = "Um dos mais famosos romances de Agatha Christie, onde Hercule Poirot investiga um assassinato em um trem.",
            PublishedDate = new DateTime(1934, 1, 1, 0, 0, 0, DateTimeKind.Utc),
            ISBN = "978-85-359-0125-9",
            PageCount = 256,
            Publisher = "Collins Crime Club",
            AuthorId = Guid.Parse("550e8400-e29b-41d4-a716-446655440004"),
            GenreId = Guid.Parse("660e8400-e29b-41d4-a716-446655440004")
        },
        new Book
        {
            Id = Guid.Parse("770e8400-e29b-41d4-a716-446655440008"),
            Title = "E Não Sobrou Nenhum",
            Description = "Romance policial sobre dez pessoas convidadas para uma ilha onde são assassinadas uma por uma.",
            PublishedDate = new DateTime(1939, 11, 6, 0, 0, 0, DateTimeKind.Utc),
            ISBN = "978-85-359-0126-6",
            PageCount = 272,
            Publisher = "Collins Crime Club",
            AuthorId = Guid.Parse("550e8400-e29b-41d4-a716-446655440004"),
            GenreId = Guid.Parse("660e8400-e29b-41d4-a716-446655440004")
        },
        new Book
        {
            Id = Guid.Parse("770e8400-e29b-41d4-a716-446655440009"),
            Title = "Dom Casmurro",
            Description = "Romance que narra a história de Bentinho e sua paixão por Capitu, uma das obras mais importantes da literatura brasileira.",
            PublishedDate = new DateTime(1899, 12, 1, 0, 0, 0, DateTimeKind.Utc),
            ISBN = "978-85-359-0127-3",
            PageCount = 208,
            Publisher = "H. Garnier",
            AuthorId = Guid.Parse("550e8400-e29b-41d4-a716-446655440005"),
            GenreId = Guid.Parse("660e8400-e29b-41d4-a716-446655440005")
        },
        new Book
        {
            Id = Guid.Parse("770e8400-e29b-41d4-a716-446655440010"),
            Title = "O Cortiço",
            Description = "Romance naturalista que retrata a vida em um cortiço no Rio de Janeiro do século XIX.",
            PublishedDate = new DateTime(1890, 1, 1, 0, 0, 0, DateTimeKind.Utc),
            ISBN = "978-85-359-0128-0",
            PageCount = 304,
            Publisher = "B. L. Garnier",
            AuthorId = Guid.Parse("550e8400-e29b-41d4-a716-446655440005"),
            GenreId = Guid.Parse("660e8400-e29b-41d4-a716-446655440005")
        },
        new Book
        {
            Id = Guid.Parse("770e8400-e29b-41d4-a716-446655440011"),
            Title = "It - A Coisa",
            Description = "Romance de terror sobre um grupo de crianças que enfrenta uma entidade maligna na cidade fictícia de Derry.",
            PublishedDate = new DateTime(1986, 9, 15, 0, 0, 0, DateTimeKind.Utc),
            ISBN = "978-85-359-0129-7",
            PageCount = 1138,
            Publisher = "Viking Press",
            AuthorId = Guid.Parse("550e8400-e29b-41d4-a716-446655440006"),
            GenreId = Guid.Parse("660e8400-e29b-41d4-a716-446655440006")
        },
        new Book
        {
            Id = Guid.Parse("770e8400-e29b-41d4-a716-446655440012"),
            Title = "O Iluminado",
            Description = "Romance de terror psicológico sobre um escritor que se torna caseiro de um hotel isolado durante o inverno.",
            PublishedDate = new DateTime(1977, 1, 28, 0, 0, 0, DateTimeKind.Utc),
            ISBN = "978-85-359-0130-3",
            PageCount = 447,
            Publisher = "Doubleday",
            AuthorId = Guid.Parse("550e8400-e29b-41d4-a716-446655440006"),
            GenreId = Guid.Parse("660e8400-e29b-41d4-a716-446655440006")
        },
        new Book
        {
            Id = Guid.Parse("770e8400-e29b-41d4-a716-446655440013"),
            Title = "Orgulho e Preconceito",
            Description = "Romance que narra a história de Elizabeth Bennet e sua relação com o orgulhoso Sr. Darcy.",
            PublishedDate = new DateTime(1813, 1, 28, 0, 0, 0, DateTimeKind.Utc),
            ISBN = "978-85-359-0131-0",
            PageCount = 432,
            Publisher = "T. Egerton",
            AuthorId = Guid.Parse("550e8400-e29b-41d4-a716-446655440007"),
            GenreId = Guid.Parse("660e8400-e29b-41d4-a716-446655440007")
        },
        new Book
        {
            Id = Guid.Parse("770e8400-e29b-41d4-a716-446655440014"),
            Title = "Emma",
            Description = "Romance sobre uma jovem bem-intencionada mas equivocada que tenta fazer de casamenteira.",
            PublishedDate = new DateTime(1815, 12, 23, 0, 0, 0, DateTimeKind.Utc),
            ISBN = "978-85-359-0132-7",
            PageCount = 474,
            Publisher = "John Murray",
            AuthorId = Guid.Parse("550e8400-e29b-41d4-a716-446655440007"),
            GenreId = Guid.Parse("660e8400-e29b-41d4-a716-446655440007")
        },
        new Book
        {
            Id = Guid.Parse("770e8400-e29b-41d4-a716-446655440015"),
            Title = "O Alquimista",
            Description = "Romance filosófico que narra a jornada de um jovem pastor andaluz em busca de seu tesouro pessoal.",
            PublishedDate = new DateTime(1988, 1, 1, 0, 0, 0, DateTimeKind.Utc),
            ISBN = "978-85-359-0133-4",
            PageCount = 163,
            Publisher = "Rocco",
            AuthorId = Guid.Parse("550e8400-e29b-41d4-a716-446655440008"),
            GenreId = Guid.Parse("660e8400-e29b-41d4-a716-446655440008")
        },
        new Book
        {
            Id = Guid.Parse("770e8400-e29b-41d4-a716-446655440016"),
            Title = "Brida",
            Description = "Romance sobre uma jovem irlandesa que explora seus dons mágicos e sua busca espiritual.",
            PublishedDate = new DateTime(1990, 1, 1, 0, 0, 0, DateTimeKind.Utc),
            ISBN = "978-85-359-0134-1",
            PageCount = 208,
            Publisher = "Rocco",
            AuthorId = Guid.Parse("550e8400-e29b-41d4-a716-446655440008"),
            GenreId = Guid.Parse("660e8400-e29b-41d4-a716-446655440008")
        }
    };
}