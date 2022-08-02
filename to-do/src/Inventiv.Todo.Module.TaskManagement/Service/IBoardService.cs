using System.Collections.Generic;
using Gazel.DataAccess;
using Inventiv.Todo.Module.TaskManagement.Security;

namespace Inventiv.Todo.Module.TaskManagement.Service
{
	public interface IBoardManagerService
	{
		// POST /boards
		// Content-Type = application/json
		// {
		//		"name": string
		// }
		IBoardInfo CreateBoard(string name);
	}

	public interface IBoardService
	{
		// PATCH /boards/{id}
		// Content-Type = application/json
		// {
		//		"name": string
		// }
		void Update(string name);

		// POST /boards/{board_id}/columns
		// Content-Type = application/json
		// {
		//		"name": string
		// }
		IColumnInfo AddColumn(string name);

		// GET /boards/{board_id}/columns 
		List<IColumnInfo> GetColumns();

		// POST /boards/{board_id}/users
		// Content-Type = application/json
		// {
		//		"user_id": int
		// }
		void AddUser(User user);

		// GET /boards/{board_id}/users
		List<IUserInfo> GetUsers();

		// DELETE /boards/{board_id}/users/{user_id}
		void RemoveUser(User user);

		// DELETE /boards/{board_id}
		void Delete();
	}

	public interface IBoardsService : IQuery
	{
		// GET /boards?name={name}
		List<IBoardInfo> GetBoards(string name);

		// GET /boards/{board_id}
		IBoardDetail GetBoard(int boardId);
	}

	public interface IBoardInfo
	{
		int Id { get; }
		string Name { get; }
	}

	public interface IBoardDetail
	{
		int Id { get; }
		string Name { get; }
		List<IColumnDetail> Columns { get; } //Eager fetching in web service layer
		List<IUserInfo> Users { get; }  //Eager fetching in web service layer
	}
}
