
using System.Threading.Tasks;

/**
 * Author: Nomi Lakkala
 * 
 * Result of a login attempt
 */
public enum LoginResult
{
	IncorrectCredentials,
	Success,
	Error
}

/**
 * Author: Nomi Lakkala
 * 
 * An interface for login providers. 
 */
public interface ILoginProvider
{
	/**
	 * Returns a <see cref="LoginResult"/> based on the success of the login attempt.
	 */
	Task<LoginResult> Login(string username, string password);

	Task Logout();
}

