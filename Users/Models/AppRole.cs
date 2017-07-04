using Microsoft.AspNet.Identity.EntityFramework;

namespace Users.Models
{


    /*
        Свойства и методы, определенные в классе RoleManager<T>
        Название	Описание
        CreateAsync(role)	
        Создает новую роль

        DeleteAsync(role)	
        Удаляет указанную роль

        FindByIdAsync(id)	
        Поиск роли по идентификатору

        FindByNameAsync(name)	
        Поиск роли по названию

        RoleExistsAsync(name)	
        Возвращает true, если существует роль с указанным именем

        UpdateAsync(role)	
        Сохраняет изменения в указанной роли

        Roles	
        Список существующих ролей     
         */
    public class AppRole : IdentityRole
    {
        public AppRole() : base() { }

        public AppRole(string name)
            : base(name)
        { }
    }
}