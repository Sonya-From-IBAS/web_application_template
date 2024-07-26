namespace MyServer.Controllers.ActionResults
{
    /// <summary>
    /// Перечисление вариантов ответа сервера 
    /// </summary>
    public enum ResponseTypeEnum
    {
        /// <summary>
        /// Неизвестный тип ошибки
        /// </summary>
        Unknown = 0,

        /// <summary>
        /// Ok
        /// </summary>
        Ok,

        /// <summary>
        /// Предупреждение
        /// </summary>
        Warning,

        /// <summary>
        /// Ошибка
        /// </summary>
        Error
    }
}
