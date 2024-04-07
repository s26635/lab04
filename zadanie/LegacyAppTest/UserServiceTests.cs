using LegacyApp;

namespace LegacyAppTest;

public class UserServiceTests
{
    [Fact]
    public void AddUser_Should_Return_False_When_Email_Without_At_And_Dot()
    {
        //Arrange
        string firstName = "John";
        string lastName = "Doe";
        DateTime birthDate = new DateTime(1980, 1, 1);
        int clientId = 1;
        string email = "doe";
        var service = new UserService();

        //Act
        bool result = service.AddUser(firstName, lastName, email, birthDate, clientId);

        //Assert
        Assert.False(result);
    }

    [Fact]
    public void AddUser_Should_Return_False_When_Missing_FirstName()
    {
        //Arrange
        string firstName = null;
        string lastName = "Doe";
        DateTime birthDate = new DateTime(1980, 1, 1);
        int clientId = 1;
        string email = "doe@example.com";
        var service = new UserService();

        //Act
        bool result = service.AddUser(firstName, lastName, email, birthDate, clientId);

        //Assert
        Assert.False(result);
    }

    [Fact]
    public void AddUser_Should_Return_False_When_Missing_LastName()
    {
        //Arrange
        string firstName = "John";
        string lastName = null;
        DateTime birthDate = new DateTime(1980, 1, 1);
        int clientId = 1;
        string email = "doe@example.com";
        var service = new UserService();

        //Act
        bool result = service.AddUser(firstName, lastName, email, birthDate, clientId);

        //Assert
        Assert.False(result);
    }

    [Fact]
    public void AddUser_Should_Return_False_When_Client_Is_Under_21_Years_Old()
    {
        //Arrange
        string firstName = "John";
        string lastName = "Doe";
        DateTime birthDate = DateTime.Now.AddYears(20);
        int clientId = 1;
        string email = "doe@example.com";
        var service = new UserService();

        //Act
        bool result = service.AddUser(firstName, lastName, email, birthDate, clientId);

        //Assert
        Assert.False(result);
    }

    [Fact]
    public void AddUser_Should_Return_False_When_Normal_Client_With_No_Credit_Limit()
    {
        //Arrange
        string firstName = "John";
        string lastName = "Kowalski";
        DateTime birthDate = new DateTime(1980, 1, 1);
        int clientId = 1;
        string email = "doe@example.com";
        var service = new UserService();

        bool result = service.AddUser(firstName, lastName, email, birthDate, clientId);
        Assert.False(result);
    }

    [Fact]
    public void AddUser_Should_Return_True_When_Important_Client()
    {
        string firstName = "John";
        string lastName = "Doe";
        DateTime birthDate = new DateTime(1980, 1, 1);
        int clientId = 4;
        string email = "doe@gmail.pl";
        var service = new UserService();
        
        //Act
        bool result = service.AddUser(firstName, lastName, email, birthDate, clientId);
        
        //Assert
        Assert.True(result);
    }
    [Fact]
    public void AddUser_Should_Return_True_When_Normal_Client()
    {
        string firstName = "John";
        string lastName = "Kwiatkowski";
        DateTime birthDate = new DateTime(1980, 1, 1);
        int clientId = 5;
        string email = "doe@gmail.pl";
        var service = new UserService();
        
        //Act
        bool result = service.AddUser(firstName, lastName, email, birthDate, clientId);
        
        //Assert
        Assert.True(result);
    }
    [Fact]
    public void AddUser_Should_Return_True_When_Very_Important_Client()
    {
        string firstName = "John";
        string lastName =  "Andrzejewicz";
        DateTime birthDate = new DateTime(1980, 1, 1);
        int clientId = 7;
        string email = "doe@gmail.pl";
        var service = new UserService();
        
        //Act
        //Assert
        try
        {
            bool result = service.AddUser(firstName, lastName, email, birthDate, clientId);
            Assert.Fail("Powinno być excetpion");
        }
        catch (ArgumentException e)
        {
            
        }
    }
    [Fact]
    public void AddUser_Should_Throw_Exception_When_User_Does_Not_Exist()
    {
        string firstName = "John";
        string lastName =  "Andrzejewicz";
        DateTime birthDate = new DateTime(1980, 1, 1);
        int clientId = 6;
        string email = "andrzejewicz@wp.pl";
        var service = new UserService();
        
        //Act
        //Assert
        try
        {
            bool result = service.AddUser(firstName, lastName, email, birthDate, clientId);
            Assert.Fail("Powinno być excetpion");
        }
        catch (ArgumentException e)
        {
            Assert.Equal($"Client {lastName} does not exist", e.Message);
        }
    }

}