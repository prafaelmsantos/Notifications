namespace Notifications.GrpcServer.Builders
{
    public static class NotificationsGrpcBuilder
    {
        public static async Task CreateNotificationsGrpcProtoFileAsync()
        {
            SchemaGenerator schemaGenerator = new() { ProtoSyntax = ProtoBuf.Meta.ProtoSyntax.Proto3 };

            var schema = schemaGenerator.GetSchema<INotificationsGrpcServerService>();

            using var writer = new StreamWriter("Notifications.Lib.gRPC.proto");
            await writer.WriteAsync(schema);
        }
    }
}
