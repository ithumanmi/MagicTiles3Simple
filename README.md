# MepPlatform Rhythm Game

Đây là một project game âm nhạc (rhythm game) được phát triển bằng Unity. Người chơi cần chạm vào các ô (tile) đúng lúc khi chúng di chuyển trên màn hình theo giai điệu của bản nhạc.

---

## 🚀 Hướng dẫn cài đặt và chạy project

1.  **Clone Repository:**
    ```bash
    git clone <your-repository-link>
    ```
2.  **Mở Project trong Unity:**
    *   Sử dụng Unity Hub để mở project.
    *   Phiên bản Unity được đề xuất: **2021.3.45f1** hoặc mới hơn.
    *   Unity sẽ mất một lúc để import project và các package lần đầu.
3.  **Chạy Game:**
    *   Mở scene chính tại: `Assets/Scenes/main.unity`.
    *   Nhấn nút **Play** trong Unity Editor để bắt đầu chơi.

---

## 🛠️ Lựa chọn thiết kế (Design Choices)

Dự án được xây dựng dựa trên các mẫu thiết kế phổ biến và hiệu quả trong phát triển game bằng Unity.

*   **Singleton Pattern (`GameManager`, `TileSpawner`):**
    *   Sử dụng Singleton để đảm bảo chỉ có một thực thể của các manager chính tồn tại và có thể được truy cập dễ dàng từ bất kỳ đâu trong project (ví dụ: `GameManager.Instance`). Điều này giúp quản lý trạng thái game (điểm số, combo, win/loss) một cách tập trung.

*   **Object Pooling (`TilePooler`):**
    *   Thay vì liên tục `Instantiate` và `Destroy` các tile, project sử dụng một hệ thống "pool" để tái sử dụng chúng. Việc này giúp giảm đáng kể lượng rác (garbage) sinh ra, tránh hiện tượng giật lag (stutter) do Garbage Collection, điều tối quan trọng đối với một game đòi hỏi sự chính xác về thời gian như rhythm game.

*   **Độ khó động (Dynamic Difficulty):**
    *   Game có thể tự động tăng độ khó theo thời gian bằng cách giảm `beatInterval` (tốc độ ra tile) trong `GameManager`.
    *   Cung cấp tùy chọn `IsEasy` để tắt tính năng này, cho phép người chơi trải nghiệm ở một tốc độ không đổi.

*   **Kiến trúc dựa trên Component (Component-Based Architecture):**
    *   Tuân thủ triệt để kiến trúc của Unity, mỗi đối tượng trong game được cấu thành từ các component độc lập (ví dụ: `TileController`, `SpriteRenderer`, `Collider2D`). Logic game được chia nhỏ vào các script khác nhau, giúp dễ quản lý và tái sử dụng.

*   **Quản lý tập trung:**
    *   Các thành phần cốt lõi như `GameManager`, `UIManager`, `TileSpawner`, `MusicManager` được thiết kế để quản lý các khía cạnh riêng biệt của game, giúp code có tổ chức và dễ bảo trì.

---

## 📦 Assets và thư viện bên ngoài

Dự án có sử dụng các assets từ bên thứ ba:

1.  **Epic Toon FX:**
    *   **Nguồn:** Unity Asset Store.
    *   **Sử dụng:** Cung cấp các hiệu ứng hình ảnh (visual effects) như cháy nổ, lấp lánh để tăng tính thẩm mỹ cho game.

2.  **Demigiant DOTween:**
    *   **Nguồn:** Unity Asset Store / [dotween.demigiant.com](http://dotween.demigiant.com/).
    *   **Sử dụng:** Một thư viện animation mạnh mẽ, được dùng để tạo các hiệu ứng chuyển động mượt mà cho UI và các đối tượng trong game (ví dụ: animation của tile khi được nhấn).

3.  **TextMesh Pro:**
    *   **Nguồn:** Unity Package Manager (tích hợp sẵn).
    *   **Sử dụng:** Dùng để hiển thị văn bản (điểm số, combo) với chất lượng cao và khả năng tùy biến mạnh mẽ.

---
Readme created by Gemini. 
