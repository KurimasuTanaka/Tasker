using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace Tasker.API;

[Authorize]
public class NotificationHub : Hub
{
}
