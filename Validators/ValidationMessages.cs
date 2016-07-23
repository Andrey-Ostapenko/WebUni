namespace livemenu.Validators
{
    public static class ValidationMessages
    {
        public static string Validation_FieldRequired = "Поле должно быть заполнено";
        public static string Validation_FieldMustBeUnique = "Значение поля должно быть уникальным";
        public static string Validation_SuchObjectAlreadyExists = "Такой код уже занят";
        public static string Validation_CatalogItemRegExp = "Код может содерать только латинские буквы, цифры, символы '_', '-'";

        public static string Validation_UserLoginRegExp = "Логин может содерать только латинские буквы, цифры, символ '_'";
        public static string Validation_UserPasswordRegExp = "Пароль может содерать только латинские буквы, цифры, символ '_'";
        public static string Validation_PasswordNotCorrent = "Пароль введен неверно";
        public static string Validation_PasswordAreNotEqual = "Пароли не совпадают";
        public static string Validation_EmailFormat = "E-mail имеет неверный формат";
    }
}