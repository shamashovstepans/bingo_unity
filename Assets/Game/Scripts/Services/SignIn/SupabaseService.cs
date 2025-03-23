using System;
using System.Threading;
using BingoGame.Dto;
using Cysharp.Threading.Tasks;
using Supabase;
using Supabase.Gotrue;
using Supabase.Gotrue.Interfaces;
using Supabase.Interfaces;
using Supabase.Realtime;
using Supabase.Storage;
using Unity.VisualScripting;
using UnityEngine;
using Client = Supabase.Client;

namespace BingoGame.Services
{
    internal class SupabaseService : IInitializable, IDisposable
    {
        private const string SupabaseUrl = "https://mntdtxdihceyogltguxw.supabase.co";
        private const string SupabaseKey = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJzdXBhYmFzZSIsInJlZiI6Im1udGR0eGRpaGNleW9nbHRndXh3Iiwicm9sZSI6ImFub24iLCJpYXQiOjE3NDIwMTIxMTYsImV4cCI6MjA1NzU4ODExNn0.i01YGmBUjuOqldCxsHAxFnqh1roPqLyHj7JWNdcKJtQ";

        private readonly CancellationTokenSource _cancellationTokenSource = new();
        private UniTaskCompletionSource<ISupabaseClient<User, Session, RealtimeSocket, RealtimeChannel, Bucket, FileObject>> _supabaseCompletionSource = new();

        public static SupabaseInstance SupabaseInstance { get; private set; }
        
        public static EpisodeModel CurrentEpisode { get; set; }
        
        public static IGotrueClient<User, Session> Auth { get; set; } 

        public SupabaseService()
        {
            InitializeAsync(_cancellationTokenSource.Token).Forget(Debug.LogError);
        }

        public void Initialize()
        {
            Debug.LogError("Initialize called");
        }

        public void Dispose()
        {
            _cancellationTokenSource.Cancel();
            _cancellationTokenSource.Dispose();
        }

        private async UniTask InitializeAsync(CancellationToken cancellationToken)
        {
            SupabaseOptions options = new()
            {
                AutoRefreshToken = true,
                AutoConnectRealtime = true
            };

            var client = new Client(SupabaseUrl, SupabaseKey, options);

            var supabaseClient = await client.InitializeAsync();
            cancellationToken.ThrowIfCancellationRequested();

            _supabaseCompletionSource.TrySetResult(supabaseClient);

            Debug.LogError("Initialized");
        }

        public async UniTask<SupabaseInstance> LoginAsync(Credentials credentials, CancellationToken cancellationToken)
        {
            var supabase = await _supabaseCompletionSource.Task;
            cancellationToken.ThrowIfCancellationRequested();

            var session = await supabase.Auth.SignIn(credentials.Email, credentials.Password);

            var instance = new SupabaseInstance(supabase);

            Auth = supabase.Auth;

            SupabaseInstance = instance;
            return instance;
        }

        public async UniTask SignUpAsync(Credentials credentials, CancellationToken cancellationToken)
        {
            var supabase = await _supabaseCompletionSource.Task;
            cancellationToken.ThrowIfCancellationRequested();

            await supabase.Auth.SignUp(credentials.Email, credentials.Password);
        }
    }
}