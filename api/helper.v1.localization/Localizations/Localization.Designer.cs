﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан программой.
//     Исполняемая версия:4.0.30319.42000
//
//     Изменения в этом файле могут привести к неправильной работе и будут потеряны в случае
//     повторной генерации кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace helper.v1.localization.Localizations {
    using System;
    
    
    /// <summary>
    ///   Класс ресурса со строгой типизацией для поиска локализованных строк и т.д.
    /// </summary>
    // Этот класс создан автоматически классом StronglyTypedResourceBuilder
    // с помощью такого средства, как ResGen или Visual Studio.
    // Чтобы добавить или удалить член, измените файл .ResX и снова запустите ResGen
    // с параметром /str или перестройте свой проект VS.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "17.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    public class Localization {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Localization() {
        }
        
        /// <summary>
        ///   Возвращает кэшированный экземпляр ResourceManager, использованный этим классом.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("helper.v1.localization.Localizations.Localization", typeof(Localization).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Перезаписывает свойство CurrentUICulture текущего потока для всех
        ///   обращений к ресурсу с помощью этого класса ресурса со строгой типизацией.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на Страницы с указанными параметрами не существует.
        /// </summary>
        public static string Activity_IsNotExist {
            get {
                return ResourceManager.GetString("Activity_IsNotExist", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на Описание бокса клавиатуры не валидное. Разрешённые символы: буквы, цифры. Мин. длина 3 символа.
        /// </summary>
        public static string BoxDescription_IsNotValid {
            get {
                return ResourceManager.GetString("BoxDescription_IsNotValid", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на Бокс клавиатуры с заданным именем уже существует на текущем аккаунте.
        /// </summary>
        public static string BoxTitle_IsBusy {
            get {
                return ResourceManager.GetString("BoxTitle_IsBusy", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на Наименование бокса клавиатуры не валидное. Разрешённые символы: буквы, цифры. Мин. длина 3 символа.
        /// </summary>
        public static string BoxTitle_IsNotValid {
            get {
                return ResourceManager.GetString("BoxTitle_IsNotValid", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на Типа бокса клавиатуры с заданными параметрами не существует.
        /// </summary>
        public static string BoxType_IsNotExist {
            get {
                return ResourceManager.GetString("BoxType_IsNotExist", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на Код подтверждения аккаунта был успешно отправлен на указанную почту.
        /// </summary>
        public static string Email_SendVerificationCode {
            get {
                return ResourceManager.GetString("Email_SendVerificationCode", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на Кода подтверждения регистрации с заданными параметрами не существует.
        /// </summary>
        public static string EmailCode_IsNotExist {
            get {
                return ResourceManager.GetString("EmailCode_IsNotExist", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на Код подтверждения регистрации был успешно отправлен на указанную почту.
        /// </summary>
        public static string EmailCode_IsSuccessfullSend {
            get {
                return ResourceManager.GetString("EmailCode_IsSuccessfullSend", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на KeyLab. Код подтверждения почты.
        /// </summary>
        public static string EmailVerification_EmailLabel {
            get {
                return ResourceManager.GetString("EmailVerification_EmailLabel", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на &lt;h3&gt;Код подтверждения почты для регистрации на платформе&lt;/h3&gt;&lt;b&gt;{0}&lt;/b&gt;&lt;p&gt;Код будет активен в течение 5 минут.&lt;/p&gt;&lt;p&gt;Это письмо было создано автоматически. На него отвечать не нужно.&lt;/p&gt;.
        /// </summary>
        public static string EmailVerification_EmailText {
            get {
                return ResourceManager.GetString("EmailVerification_EmailText", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на Файл повреждён или не был прикреплён.
        /// </summary>
        public static string File_IsNotAttached {
            get {
                return ResourceManager.GetString("File_IsNotAttached", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на Файла с заданными параметрами не существует.
        /// </summary>
        public static string File_IsNotExist {
            get {
                return ResourceManager.GetString("File_IsNotExist", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на Файл был успешно удалён.
        /// </summary>
        public static string File_IsSuccessfullDeleted {
            get {
                return ResourceManager.GetString("File_IsSuccessfullDeleted", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на Файл был успешно обновлён.
        /// </summary>
        public static string File_IsSuccessfullUpdated {
            get {
                return ResourceManager.GetString("File_IsSuccessfullUpdated", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на Файл был успешно сохранён.
        /// </summary>
        public static string File_IsSuccessfullUploaded {
            get {
                return ResourceManager.GetString("File_IsSuccessfullUploaded", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на Временного интервала с заданными параметрами не существует.
        /// </summary>
        public static string Interval_IsNotExist {
            get {
                return ResourceManager.GetString("Interval_IsNotExist", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на Описание клавиатуры не валидное. Разрешённые символы: буквы, цифры. Мин. длина 3 символа.
        /// </summary>
        public static string KeyboardDescription_IsNotValid {
            get {
                return ResourceManager.GetString("KeyboardDescription_IsNotValid", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на Клавиатура с заданным именем уже существует на текущем аккаунте.
        /// </summary>
        public static string KeyboardTitle_IsBusy {
            get {
                return ResourceManager.GetString("KeyboardTitle_IsBusy", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на Наименование клавиатуры не валидное. Разрешённые символы: буквы, цифры. Мин. длина 3 символа.
        /// </summary>
        public static string KeyboardTitle_IsNotValid {
            get {
                return ResourceManager.GetString("KeyboardTitle_IsNotValid", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на Клавиша с заданными параметрами не существует.
        /// </summary>
        public static string Keycap_IsNotExist {
            get {
                return ResourceManager.GetString("Keycap_IsNotExist", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на Набора кейкапов с заданными параметрами не существует.
        /// </summary>
        public static string Kit_IsNotExist {
            get {
                return ResourceManager.GetString("Kit_IsNotExist", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на Наименование набора кейкапов не валидное. Разрешённые символы: буквы, цифры. Мин. длина 3 символа.
        /// </summary>
        public static string KitTitle_IsNotValid {
            get {
                return ResourceManager.GetString("KitTitle_IsNotValid", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на Номер страницы не валидный.
        /// </summary>
        public static string PaginationPage_NotValid {
            get {
                return ResourceManager.GetString("PaginationPage_NotValid", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на Размер страницы не валидный.
        /// </summary>
        public static string PaginationPageSize_NotValid {
            get {
                return ResourceManager.GetString("PaginationPageSize_NotValid", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на Превью файла повреждено или не было прикреплено.
        /// </summary>
        public static string Preview_IsNotAttached {
            get {
                return ResourceManager.GetString("Preview_IsNotAttached", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на Левая дата периода больше правой.
        /// </summary>
        public static string Stats_LeftDateGreaterThanRightDate {
            get {
                return ResourceManager.GetString("Stats_LeftDateGreaterThanRightDate", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на Типа переключателя с заданными параметрами не существует.
        /// </summary>
        public static string SwitchType_IsNotExist {
            get {
                return ResourceManager.GetString("SwitchType_IsNotExist", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на Текущему пользователю не принадлежит бокс с указанными параметрами.
        /// </summary>
        public static string User_IsNotBoxOwner {
            get {
                return ResourceManager.GetString("User_IsNotBoxOwner", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на Пользователя с заданными параметрами не существует.
        /// </summary>
        public static string User_IsNotExist {
            get {
                return ResourceManager.GetString("User_IsNotExist", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на Текущему пользователю не принадлежит клавиатура с указанными параметрами.
        /// </summary>
        public static string User_IsNotKeyboardOwner {
            get {
                return ResourceManager.GetString("User_IsNotKeyboardOwner", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на Текущему пользователю не принадлежит набор кейкапов с указанными параметрами.
        /// </summary>
        public static string User_IsNotKitOwner {
            get {
                return ResourceManager.GetString("User_IsNotKitOwner", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на Access токен просрочен или повреждён. Пройдите заново процесс авторизации.
        /// </summary>
        public static string UserAccessToken_IsExpired {
            get {
                return ResourceManager.GetString("UserAccessToken_IsExpired", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на Заданная почта уже занята другим пользователем.
        /// </summary>
        public static string UserEmail_IsBusy {
            get {
                return ResourceManager.GetString("UserEmail_IsBusy", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на Почта не валидная. Пример: ivanov@mail.ru.
        /// </summary>
        public static string UserEmail_IsNotValid {
            get {
                return ResourceManager.GetString("UserEmail_IsNotValid", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на Никнейм не валидный. Разрешённые символы: буквы, цифры. Мин. длина 3 символа.
        /// </summary>
        public static string UserNickname_IsNotValid {
            get {
                return ResourceManager.GetString("UserNickname_IsNotValid", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на Пароль не валидный. Разрешённые символы: буквы, цифры. Мин. длина 8 символов.
        /// </summary>
        public static string UserPassword_IsNotValid {
            get {
                return ResourceManager.GetString("UserPassword_IsNotValid", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на Переданные значения паролей не совпадают.
        /// </summary>
        public static string UserPasswords_IsNotEqual {
            get {
                return ResourceManager.GetString("UserPasswords_IsNotEqual", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на Refresh токен просрочен или повреждён. Пройдите заново процесс авторизации.
        /// </summary>
        public static string UserRefreshToken_IsExpired {
            get {
                return ResourceManager.GetString("UserRefreshToken_IsExpired", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на Был совершён вход в аккаунт.
        /// </summary>
        public static string UserSignIn_EmailLabel {
            get {
                return ResourceManager.GetString("UserSignIn_EmailLabel", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на Был совершён вход в аккаунт на платформе KeyLab. Если это были не вы, то смените пароль.
        /// </summary>
        public static string UserSignIn_EmailText {
            get {
                return ResourceManager.GetString("UserSignIn_EmailText", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на Новый аккаунт был успешно зарегистрирован.
        /// </summary>
        public static string UserSignUp_IsSuccessfull {
            get {
                return ResourceManager.GetString("UserSignUp_IsSuccessfull", resourceCulture);
            }
        }
    }
}
