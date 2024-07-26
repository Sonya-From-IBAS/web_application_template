namespace MyServer.Controllers.ActionResults
{
    /// <summary>
    /// Базовый класс для ответа сервера в виде JSON
    /// </summary>
    public class JsonResponse
    {
        /// <summary>
        /// 
        /// </summary>
        private string _message;

        /// <summary>
        /// 
        /// </summary>
        private string _fullMessage;

        /// <summary>
        /// 
        /// </summary>
        private object _data;

        /// <summary>
        /// 
        /// </summary>
        private string[] _messages;

        /// <summary>
        /// Тип ответа сервера
        /// </summary>
        public ResponseTypeEnum ResponseType { get; protected set; }

        /// <summary>
        /// 
        /// </summary>
        //public bool ExistsMessage { get { return !string.IsNullOrEmpty(Message); } }

        /// <summary>
        /// 
        /// </summary>
        //public bool MultipleMessages { get { return (Messages != null) && (Message.Length > 0); } }

        /// <summary>
        /// 
        /// </summary>
        //public bool Success { get { return ResponseType == ResponseTypeEnum.Ok; } }
        
        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="responseTypeEnum">Тип ответа сервера</param>
        public JsonResponse(ResponseTypeEnum responseTypeEnum)
        {
            ResponseType = responseTypeEnum;
            _messages = [];
        }

        /// <summary>
        /// 
        /// </summary>
        public string Message
        {
            get { return _message; }
            set
            {
                if(string.IsNullOrEmpty(value)) throw new ArgumentNullException("Message");

                _message = value;
            }
        }


        /// <summary>
        /// Для вывода нескольких сообщений
        /// </summary>
        public string[] Messages
        {
            get { return _messages; }
            set
            {
                ArgumentNullException.ThrowIfNull(value);
                var messages = value.Where(x => !string.IsNullOrEmpty(x)).ToArray();
                if(messages.Length < 1) throw new ArgumentNullException("Messages");
                _messages = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string FullMessage
        {
            get { return string.IsNullOrEmpty(_fullMessage) ? _message : _fullMessage; }
            set
            {
                _fullMessage = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public object Data
        {
            get { return _data; }
            set
            {
                ArgumentNullException.ThrowIfNull(value);
                _data = value;
            }
        }
    }
}
