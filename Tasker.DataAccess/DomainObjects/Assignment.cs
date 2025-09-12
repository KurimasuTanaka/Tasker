using System;
using Tasker.Database;

namespace Tasker.DataAccess;

public class Assignment : AssignmentModel
{
	public Assignment() { }
	public Assignment(AssignmentModel model)
	{
		this.AssignmentId = model.AssignmentId;
		this.Title = model.Title;
		this.GroupId = model.GroupId;
		this.Description = model.Description;
		this.IsCompleted = model.IsCompleted;
		this.Participants = model.Participants;
	}

	public Assignment(AssignmentDTO dto)
	{
		this.AssignmentId = dto.AssignmentId;
		this.Title = dto.Title;
		this.GroupId = dto.GroupId;
		this.Description = dto.Description;
		this.IsCompleted = dto.IsCompleted;
		// this.Participants = dto.Participants.Select(au => new UserAssignmentModel
		// {
		// 	UserId = au.UserId,
		// 	AssignmentId = au.AssignmentId
		// }).ToList();
	}

	public AssignmentDTO ToDTO()
	{
		return new AssignmentDTO
		{
			AssignmentId = this.AssignmentId,
			Title = this.Title,
			Description = this.Description,
			IsCompleted = this.IsCompleted,
			GroupId = this.GroupId,
			// Participants = this.Participants.Select(p => new UserAssignment(p)).ToList()
		};
	}

	public bool HasThisUserAssigned(string userId)
	{
		return Participants.Select(p => p.User.UserIdentity).
				Contains(userId);
	}
}
