using Azure;
using Azure.AI.Inference;


namespace P2PLearningAPI.Services
{
    public class AzureService
    {
        private readonly ChatCompletionsClient _client;
        private readonly string _model;

        public AzureService()
        {
            var endpoint = new Uri("https://models.github.ai/inference");
            var token = Environment.GetEnvironmentVariable("GITHUB_TOKEN");

            if (string.IsNullOrWhiteSpace(token))
            {
                throw new InvalidOperationException("GITHUB_TOKEN environment variable is not set.");
            }

            var credential = new AzureKeyCredential(token);
            _model = "deepseek/DeepSeek-V3-0324";

            _client = new ChatCompletionsClient(endpoint, credential, new AzureAIInferenceClientOptions());
        }

        public string GetResponse(string userMessage)
        {
            var requestOptions = new ChatCompletionsOptions()
            {
                Messages =
            {
                new ChatRequestSystemMessage("You are a helpful assistant."),
                new ChatRequestUserMessage(userMessage),
            },
                Temperature = 1.0f,
                NucleusSamplingFactor = 1.0f,
                MaxTokens = 1000,
                Model = _model
            };

            Response<ChatCompletions> response = _client.Complete(requestOptions);
            return response.Value.Content;
        }
    }
}
