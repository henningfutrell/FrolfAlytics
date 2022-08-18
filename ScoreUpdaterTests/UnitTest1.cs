using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Xunit.Abstractions;

namespace ScoreSheetImporterTests;

public class UnitTest1
{
    private readonly ITestOutputHelper _output;

    public UnitTest1(ITestOutputHelper output)
    {
        _output = output;
    }

    [Fact]
    public void AddingPlayerWorks()
    {
        var options = new DbContextOptionsBuilder<FrolfContext>()
            .UseSqlite("Filename=:memory:")
            .Options;

        using var context = new FrolfContext(options);
        context.Database.OpenConnection();
        context.Database.EnsureCreated();

        var player = new Player("PlayerName");

        context.Players.Add(player);
        context.SaveChanges();

        Assert.True(context.Players.First().UserName.Equals("PlayerName"));
    }

    [Fact]
    public void AddingCourse()
    {
        var connection = new SqliteConnection("Filename=:memory:");
        connection.Open();

        var options = new DbContextOptionsBuilder<FrolfContext>()
            .UseSqlite(connection)
            .Options;

        using var context = new FrolfContext(options);
        context.Database.EnsureCreated();

        var course = new Course("Name");
        context.Courses.Add(course);
        context.SaveChanges();

        using var newContext = new FrolfContext(options);
        Assert.Equal("Name", newContext.Courses.First().Name);
    }

}

class FrolfContext : DbContext
{
    public FrolfContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<Player> Players { get; private set; }
    public DbSet<HoleScore> HoleScores { get; private set; }
    public DbSet<Hole> Holes { get; private set; }
    public DbSet<Course> Courses { get; private set; }
    public DbSet<Round> Rounds { get; private set; }
    public DbSet<Layout> Layouts { get; private set; }
    public DbSet<ScoreCard> ScoreCards { get; private set; }
}

class HoleScore
{
    private HoleScore()
    {
    }

    public HoleScore(List<Player> players, Hole hole, int score)
    {
        Players = players;
        Hole = hole;
        Score = score;
    }

    public int Id { get; private set; }
    public List<Player> Players { get; private set; }
    public Hole Hole { get; private set; }
    public int Score { get; private set; }
}

class Hole
{
    private Hole()
    {
    }

    public Hole(int number, Course course, Layout layout, int par)
    {
        Number = number;
        Course = course;
        Layout = layout;
        Par = par;
    }

    public int Id { get; private set; }
    public int Number { get; private set; }
    public Course Course { get; private set; }
    public Layout Layout { get; private set; }
    public int Par { get; private set; }
}

class ScoreCard
{
    private ScoreCard()
    {
    }

    public ScoreCard(Course course, Layout layout, Player player, List<HoleScore> holeScores)
    {
        Course = course;
        Layout = layout;
        Player = player;
        HoleScores = holeScores;
    }

    public int Id { get; private set; }
    public Course Course { get; private set; }
    public Layout Layout { get; private set; }
    public Player Player { get; private set; }
    public List<HoleScore> HoleScores { get; private set; }
}

class Layout
{
    private Layout()
    {
    }

    public Layout(string name)
    {
        Name = name;
    }

    public int Id { get; private set; }
    public string Name { get; private set; }
    public Course Course { get; private set; }
    public List<Hole> Holes { get; private set; }
}

class Course
{
    private Course()
    {
    }

    /// <summary>
    /// New Constructor
    /// </summary>
    /// <param name="name"></param>
    public Course(string name)
    {
        Name = name;
        Layouts = new List<Layout>();
    }

    public int Id { get; private set; }
    public string Name { get; private set; }
    public List<Layout> Layouts { get; private set; }
}

class Round
{
    private Round()
    {
    }

    public Round(Course course, Layout layout, DateTime date, List<Player> players, List<ScoreCard> scoreCards)
    {
        Course = course;
        Layout = layout;
        Date = date;
        Players = players;
        ScoreCards = scoreCards;
    }

    public int Id { get; private set; }
    public Course Course { get; private set; }
    public Layout Layout { get; private set; }
    public DateTime Date { get; private set; }
    public List<Player> Players { get; private set; }
    public List<ScoreCard> ScoreCards { get; private set; }
}

class Player
{
    private Player()
    {
    }

    public Player(string userName)
    {
        UserName = userName;
    }

    public int Id { get; private set; }
    public string UserName { get; private set; }
}