using System;
using Microsoft.AspNetCore.Mvc;

namespace ToDoApp.Controllers;

public class MagicController : Controller
{
    // GET: Magic Controller
    public ActionResult Index()
    {
        return View();
    }

    public IActionResult Hello(string message)
    {
        return Content(message.ToUpper());
    }

    public IActionResult Square(int number)
    {
        int square = number * number;
        return Content(square.ToString());
    }

    public IActionResult Calculator()
    {
        return View();
    }

    public IActionResult Guess()
    {


        return View();
    }

    public IActionResult GenerateNum()
    {
        int num = new Random().Next(1, 101);
        HttpContext.Response.Cookies.Append("num", $"{num}");
        return Content("");
    }

    public IActionResult CheckGuess(int num)
    {
        var needNum = int.Parse(HttpContext.Request.Cookies["num"]);
        if(needNum == num){
            return Content("Вы угадали число!");
        }
        else if(needNum >= num){
            return Content("Ваше число меньше загаданного!");
        }
        else{
            return Content("Ваше число больше загаданного!");
        }
    }

    public IActionResult Calculate(int a, int b, string op)
    {
        int res = 0;
        switch (op)
        {
            case "+":
                res = a + b;
                break;
            case "-":
                res = a - b;
                break;
            case "*":
                res = a * b;
                break;
            case "/":
                res = a / b;
                break;
        }


        return Content(res.ToString());
    }
}
