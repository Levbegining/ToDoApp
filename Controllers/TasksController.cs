using System;
using Microsoft.AspNetCore.Mvc;

namespace ToDoApp.Controllers;

public class TasksController : Controller
{
    public IActionResult Index()
    {
        //TaskRepository.Tasks.Add("hello");
        //TaskRepository.Tasks.Add("buy");


        return View();
    }
    [HttpGet]
    public IActionResult GetTasks()
    {
        // анонимный тип данных
        return Json(new { tasks = TaskRepository.Tasks });
    }

    public IActionResult AddTask(string text)
    {
        TaskRepository.Tasks.Add(text);
        var res = Json(new {tasks = TaskRepository.Tasks});
        return res;
    }
    public IActionResult RemoveTask(string txtOfTask){
        TaskRepository.Tasks.Remove(txtOfTask);
        return Content("");
    }
}

public static class TaskRepository
{
    public static List<string> Tasks = new List<string>();
}
