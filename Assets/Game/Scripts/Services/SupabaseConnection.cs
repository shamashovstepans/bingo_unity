using System.Threading;
using BingoGame.Dto;
using Cysharp.Threading.Tasks;
using Supabase;
using UnityEngine;
using Client = Supabase.Client;

namespace BingoGame.Services
{
    internal class SupabaseConnection
    {
        private const string SupabaseUrl = "https://mntdtxdihceyogltguxw.supabase.co";
        private const string SupabaseKey = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJzdXBhYmFzZSIsInJlZiI6Im1udGR0eGRpaGNleW9nbHRndXh3Iiwicm9sZSI6ImFub24iLCJpYXQiOjE3NDIwMTIxMTYsImV4cCI6MjA1NzU4ODExNn0.i01YGmBUjuOqldCxsHAxFnqh1roPqLyHj7JWNdcKJtQ";

        private Client _supabase;

        public SupabaseConnection()
        {
        }

        public async UniTask ConnectAsync(CancellationToken cancellationToken)
        {
            SupabaseOptions options = new()
            {
                AutoRefreshToken = true,
                AutoConnectRealtime = true
            };

            _supabase = new Client(SupabaseUrl, SupabaseKey, options);

            var client = await _supabase.InitializeAsync();
            cancellationToken.ThrowIfCancellationRequested();

            var episodesResult = await client.From<EpisodeModel>().Get(cancellationToken);
            foreach (var episodeModel in episodesResult.Models)
            {
                Debug.LogError($"Episode: {episodeModel.Name}");
                Debug.LogError($"Link: {episodeModel.Link}");
            }
            
            var session = await client.Auth.SignIn("shamashov.stepans@gmail.com", "12345678");
            Debug.LogError(session.User.Email);
            
            var result = await client.From<EpisodeModel>().Where(model => model.Id == 1).Set(x => x.Name, "New episode").Update(cancellationToken: cancellationToken);
            Debug.LogError($"Updated succsessfully, affected rows: {result.ResponseMessage}");

            var result2 = await client.From<EpisodeModel>().Insert(new EpisodeModel() { Name = "Test", Link = "N/A" }, null, cancellationToken);
            Debug.LogError($"Inserted episode: {result2.ResponseMessage}");
        }
    }
}