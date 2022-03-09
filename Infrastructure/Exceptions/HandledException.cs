using Infrastructure.Interfaces;

namespace Infrastructure.Exceptions
{
    public class HandledException : System.Exception, IHandledException
    {
        public HandledException(string message)
            : base(message)
        {
        }
    }

    public class TaskIsEndAlredy : System.Exception, IHandledException
    {
        public TaskIsEndAlredy(string message)
            : base(message)
        {
        }
    }

    public class TasHasNotStartedYet : System.Exception, IHandledException
    {
        public TasHasNotStartedYet(string message)
            : base(message)
        {
        }
    }


    public class UserIsBlocked : System.Exception, IHandledException
    {
        public UserIsBlocked(string message)
            : base(message)
        {
        }
    }

    public class TaskIsStartedAlredy : System.Exception, IHandledException
    {
        public TaskIsStartedAlredy(string message)
            : base(message)
        {
        }
    }
 
    public class SpecificEmployeeNotFound : System.Exception, IHandledException
    {
        public SpecificEmployeeNotFound(string message)
            : base(message)
        {
        }
    }  
    
    public class PasswordIsWrong : System.Exception, IHandledException
    {
        public PasswordIsWrong(string message)
            : base(message)
        {
        }
    }

    public class DependencyViolation : System.Exception, IHandledException
    {
        public DependencyViolation(string message)
            : base(message)
        {
        }
    }

    public class RefreshTokenHasBeenExpired : System.Exception, IHandledException
    {
        public RefreshTokenHasBeenExpired(string message)
            : base(message)
        {
        }
    }

    public class TokenNotExpiredYet : System.Exception, IHandledException
    {
        public TokenNotExpiredYet(string message)
            : base(message)
        {
        }
    }


    public class RefreshTokenHasBeenChanged : System.Exception, IHandledException
    {
        public RefreshTokenHasBeenChanged(string message)
            : base(message)
        {
        }
    }

    public class AlreadyLoggedIn : System.Exception, IHandledException
    {
        public AlreadyLoggedIn(string message)
            : base(message)
        {
        }
    }

    public class Authorization : System.Exception, IHandledException
    {
        public Authorization(string message)
            : base(message)
        {
        }
    } 
    
    public class UserPassWrong : System.Exception, IHandledException
    {
        public UserPassWrong(string message)
            : base(message)
        {
        }
    }

    public class InvalidFromat : System.Exception, IHandledException
    {
        public InvalidFromat(string message)
            : base(message)
        {
        }
    }

    public class NotFount : System.Exception, IHandledException
    {
        public NotFount(string message)
            : base(message)
        {
        }
    } 
    
    
    public class InvalidToken : System.Exception, IHandledException
    {
        public InvalidToken(string message)
            : base(message)
        {
        }
    }  
    
    public class UserWithoutAnyRole : System.Exception, IHandledException
    {
        public UserWithoutAnyRole(string message)
            : base(message)
        {
        }
    }
}