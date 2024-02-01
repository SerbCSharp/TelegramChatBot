using Microsoft.EntityFrameworkCore;
using TelegramChatBot.Model;

namespace TelegramChatBot.Infrastructure.Repositories
{
    public class RequestRepository
    {
        private readonly StatisticsContext _context;
        public RequestRepository(StatisticsContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<int> CreateRequestAsync(Request request)
        {
            _context.Requests.Add(request);
            return await _context.SaveChangesAsync();
        }

        public async Task<List<Request>> GetRequestAsync(int skip, int take)
        {
            return await _context.Requests.Skip(skip).Take(take).ToListAsync();
        }
    }
}
