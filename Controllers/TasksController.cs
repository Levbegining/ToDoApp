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
        var res = Json(new { tasks = TaskRepository.Tasks });
        return res;
    }
    public IActionResult RemoveTask(int id)
    {
        TaskRepository.Tasks.RemoveAt(id);
        return Content("");
    }
    public IActionResult Replace(int id, int idTwo)
    {
        string el = TaskRepository.Tasks[id];
        TaskRepository.Tasks.RemoveAt(id);

        TaskRepository.Tasks.Insert(idTwo, el);
        return Content("");
    }
}

public static class TaskRepository
{
    public static List<string> Tasks = new List<string>();
}
