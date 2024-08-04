namespace MyServer.Models
{
    /// <summary>
    /// Класс настроек для отправки email
    /// </summary>
    public class EmailSettings
    {
        /// <summary>
        /// Имя отправителя
        /// </summary>
        public string SenderName { get; set; }

        /// <summary>
        /// Адрес электронной почты отправителя
        /// </summary>
        public string SenderAddress { get; set; }

        /// <summary>
        /// Адресс электронной почты, на которую привязана почтовая программа
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// Пароль приложения почта (SMTP)
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Хост яндекса - smtp.yandex.ru
        /// </summary>
        public string Host { get; set; }

        /// <summary>
        /// Порт, у Яндекса 465 (с SSL шифрованием)
        /// </summary>
        public int Port { get; set; }


        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="displayName"></param>
        /// <param name="from"></param>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <param name="host"></param>
        /// <param name="port"></param>
        /// <param name="useSSL"></param>
        /// <param name="useStartTls"></param>
        /// <param name="useOAuth"></param>
        public EmailSettings(
            string senderName,
            string senderAddress,
            string userName,
            string password,
            string host,
            int port)
        {
            SenderName = senderName;
            SenderAddress = senderAddress;
            UserName = userName;
            Password = password;
            Host = host;
            Port = port;
        }

    }
}
