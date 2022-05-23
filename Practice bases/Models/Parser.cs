namespace Practice_bases.Models;

public class Parser
{
    private ApplicationContext _db;

    public Parser(ApplicationContext _db)
    {
        this._db = _db;
    }

    public Human parseFIO(object obj, Type type)
    {
        if (obj == null)
            obj = "Не указано";
        
        string value = obj.ToString();
        Human o = _db.Humans.FirstOrDefault(x => x.Name.Equals(value));
        if (o != null)
            return o;
        return new Human {Name = value, Type = type};
    }
    

    public Group parseGroup(object obj)
    {
        if (obj == null)
            obj = "Не указано";
        
        string value = obj.ToString();
        Group o = _db.Groups.FirstOrDefault(x => x.Title.Equals(value)); 
        if (o != null)
            return o;
        return new Group {Title = value};
    }

    public Address ParceAddress(object obj)
    {
        if (obj == null)
            obj = "Не указано";
        
        string value = obj.ToString();
        Address o = _db.Address.FirstOrDefault(x => x.Title.Equals(value));
        if (o != null)
            return o;
        return new Address {Title = value};
    }

    public Mail ParceMail(object obj, Type type)
    {
        if (obj == null)
            obj = "Не указано";
        
        string value = obj.ToString();
        Mail o = _db.Mails.FirstOrDefault(x => x.Title.Equals(value));
        if (o != null)
            return o;
        return new Mail {Title = value, Type = type};
    }
    
    public Organization ParceOrganization(object obj)
    {
        if (obj == null)
            obj = "Не указано";
        
        string value = obj.ToString();
        Organization o = _db.Organizations.FirstOrDefault(x => x.Title.Equals(value));
        if (o != null)
            return o;
        return new Organization {Title = value};
    }
    
    public Phone ParcePhone(object obj, Type type)
    {
        if (obj == null)
            obj = "Не указано";
        
        string value = obj.ToString();
        Phone o = _db.Phones.FirstOrDefault(x => x.Title.Equals(value));
        if (o != null)
            return o;
        return new Phone {Title = value, Type = type};
    }
    
    public Website ParceWebsite(object obj)
    {
        if (obj == null)
            obj = "Не указано";
        
        string value = obj.ToString();
        Website o = _db.Websites.FirstOrDefault(x => x.Title.Equals(value));
        if (o != null)
            return o;
        return new Website {Title = value};
    }
}