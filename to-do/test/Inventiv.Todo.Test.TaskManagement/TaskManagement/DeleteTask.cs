using NUnit.Framework;

namespace Inventiv.ToDo.Test.UnitTest.TaskManagement
{
	[TestFixture]
	public class DeleteTask : ToDoTestBase
	{
		[Test]
		public void GIVEN_there_exists_a_task__WHEN_user_deletes_the_task__THEN_then_it_is_removed_from_system()
		{
			var task = CreateTask();

			BeginTest();

			task.Delete();

			Verify.ObjectIsDeleted(task);
		}
	}
}
