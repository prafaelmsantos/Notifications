namespace Notifications.Core
{
    public static class DomainResource
    {
        #region Client Message

        public static readonly string ClientMessageNotFoundException = "Mensagem de cliente não encontrada.";

        public static readonly string ClientMessageIdNeedsToBeSpecifiedException = "O id da marca é invalido.";
        public static readonly string ClientMessageNameNeedsToBeSpecifiedException = "O nome da mensagem de cliente é invalido.";
        public static readonly string ClientMessageEmailNeedsToBeSpecifiedException = "O email da mensagem de cliente é invalido.";
        public static readonly string ClientMessagePhoneNumberNeedsToBeSpecifiedException = "O contacto da mensagem de cliente é invalido.";
        public static readonly string ClientMessageNeedsToBeSpecifiedException = "A mensagem de cliente é invalida.";
        public static readonly string ClientMessageStatusNeedsToBeSpecifiedException = "O estado da mensagem de cliente é invalido.";

        public static readonly string GetAllClientMessagesAsyncException = "Erro ao tentar encontrar mensagens de clientes.";
        public static readonly string GetClientMessageByIdAsyncException = "Erro ao tentar encontrar mensagem de cliente por id.";
        public static readonly string AddClientMessageAsyncException = "Erro ao tentar criar a mensagem de cliente.";
        public static readonly string UpdateClientMessageAsyncException = "Erro ao tentar atualizar a mensagem de cliente.";
        public static readonly string DeleteClientMessagesAsyncException = "Erro ao tentar encontrar apagar mensagens de clientes.";

        #endregion

        #region Visitor

        public static readonly string GetVisitorCountersAsyncException = "Erro ao tentar encontrar o numero total de visitantes.";
        public static readonly string GetAllVisitorsWithYearComparisonAsyncException = "Erro ao tentar encontrar o numero total (por ano) de visitantes.";
        public static readonly string GetAllVisitorsWithMonthComparisonAsyncException = "Erro ao tentar encontrar o numero total (por mês) de visitantes.";
        public static readonly string CreateOrUpdateVisitorAsyncException = "Erro ao tentar criar/editar o numero total de visitantes.";

        #endregion
    }
}
